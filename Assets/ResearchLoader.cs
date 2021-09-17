using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CatchyClick;
using Dummiesman;
using UnityEngine.UI;

public class ResearchLoader : MonoBehaviour
{
    public RectTransform mainCanvas;
    public string slicerPath = "H:/Program Files/Slicer 4.11.20210226";
    public string outputPath = "H:/tmp";
    public DataGridView dataGrid;
    public RectTransform progressWindowTemplate;
    public MeshController meshController;
    public GameObject meshes;
    public SeriesController seriesController;
    public List<MeshData> meshesData;
    public MeshFilterController mFController;

    public Text dateLabel;
    public Text stateLabel;

    public ImagesLoader imagesLoader;

    public CanvasController canvasController;

    public UnityEvent researchLoaded = new UnityEvent();
    public UnityEvent researchLoadFailed = new UnityEvent();

    private LoadingModalWindow progressWindow;
    private DataGridViewEventArgs args;

    private string researchDirPath;

    void Start()
    {
        dataGrid = GetComponent<DataGridView>();
        dataGrid.cellClicked.AddListener(OnCellClicked);
        researchLoaded.AddListener(OnResearchLoaded);
        researchLoadFailed.AddListener(OnReasearchLoadFailed);
    }

    async Task<bool> _LoadResearch(int researchId)
    {
        Research research = await DBResearches.GetFullResearchInfo(researchId);

        if (research == null || research.series.Count == 0)
        {
            return false;
        }

        Series series = research.series[seriesController.current];

        GameObject modalWindowInstance = Instantiate(progressWindowTemplate.gameObject, mainCanvas);
        if (modalWindowInstance)
        {
            progressWindow = modalWindowInstance.GetComponent<LoadingModalWindow>();
        }

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        researchDirPath = $"{outputPath}/research_{researchId}_series_{series.id}";

        if (!Directory.Exists(researchDirPath))
        {
            Directory.CreateDirectory(researchDirPath);
        }

        List<string> files = new List<string>() {
            "scripts/python/GenerateMesh",
            "scripts/python/Segmenter",
            "scripts/python/slicerApp",
            "scripts/python/test",
            "scripts/python/SimpleITK/ExportImages",
            "scripts/python/SimpleITK/SimpleITKImages"
        };
        bool extractionResult = files.Select(f => ResourcesExtractor.ExtractTextAsset(f, "py")).All(e => e == true);

        if (!extractionResult)
        {
            researchLoadFailed.Invoke();
            return false;
        }

        string researchImagesDirPath = $"{researchDirPath}/images";

        //Images
        if (!Directory.Exists(researchImagesDirPath))
        {
            progressWindow.bodyText.text = $"Экспорт снимков КТ из {series.sourceNiiFilePath}";

            bool imagesExported = await NiiImagesExporter.Export(series.sourceNiiFilePath, researchDirPath);

            if (!imagesExported)
            {
                researchLoadFailed.Invoke();
                return false;
            }
        }

        Texture2D[] thumbnails = await GetThumbnails(research);
        List<int> photosCount = GetPhotosCount(research);

        for (int i = 0; i < research.series.Count; i++)
        {
            Series s = research.series[i];
            s.thumbnail = thumbnails[i];
            s.photosCount = photosCount[i];
        }

        progressWindow.bodyText.text = $"Чтение снимков КТ";

        List<string> directories = Directory.GetDirectories($"{researchDirPath}/images").ToList();

        if (directories.Count > 0)
        {
            Texture2D[][] textures = await imagesLoader.GetImagesFromDirectories(directories);

            for (int i = 0; i < directories.Count; i++)
            {
                imagesLoader.viewers[i].SetImages(textures[i].ToList());
            }
        }

        //Parts
        List<Process> processes = GetSlicerProcesses();
        if (processes.Count > 0) CloseProcesses(processes);

        progressWindow.bodyText.text = $"Проверка кэша";

        meshesData = GetMeshesData(series.parts);

        bool meshesExists = series.parts
            .Select(p => File.Exists(p.meshFilePath))
            .All(e => e == true);

        if (!meshesExists)
        {
            progressWindow.bodyText.text = $"Генерация мешей";
            bool generationFinished = await GenerateMeshes(meshesData);

            if (generationFinished)
            {
                bool partsUpdated = await UpdatePartsData(series.parts);

                if (partsUpdated)
                {
                    research = await DBResearches.GetFullResearchInfo(researchId);
                }
            }
        }

        progressWindow.bodyText.text = $"Загрузка мешей";
        bool meshesLoaded = LoadMeshes(series.parts);

        if (meshesLoaded)
        {
            seriesController.AddSeriesRange(research.series);
            seriesController.seriesChanged.AddListener(OnSeriesChanged);
            stateLabel.text = Research.GetStringFromState(research.state);
            return true;
        }
        else
        {

        }

        return false;
    }

    public async void LoadResearch()
    {
        DataGridViewRow row = dataGrid.rows[args.row];
        int index;

        if (Int32.TryParse(row.cells[0].value, out index))
        {
            bool loadResult = await _LoadResearch(index);
            if (loadResult)
            {
                UnityEngine.Debug.Log("Finished");
                researchLoaded.Invoke();
                return;
            }
        }
        else
        {

        }

        researchLoadFailed.Invoke();
    }

    private List<Process> GetSlicerProcesses()
    {
        Process[] slicers = Process.GetProcessesByName("Slicer");
        Process[] slicerApps = Process.GetProcessesByName("SlicerApp-real");
        List<Process> processes = new List<Process>();

        processes.AddRange(slicers.ToList());
        processes.AddRange(slicerApps.ToList());

        return processes;
    }

    private static void CloseProcesses(List<Process> processes)
    {
        processes.ForEach(p =>
        {
            p.CloseMainWindow();
            p.Close();
        });
    }

    private async Task<bool> UpdatePartsData(List<Part> parts)
    {
        List<Task<bool>> tasks = meshesData.Select(async m =>
        {
            int id = parts.Find(p => p.filePath == m.inputFilePath).id;
            Dictionary<string, string> dictionary = new Dictionary<string, string>() { { "meshFilePath", $"{m.outputFilePath}/Segmentation.obj" } };
            QueryBuilder queryBuilder = new QueryBuilder(dictionary);
            return await DBParts.EditPart(id, queryBuilder);
        }).ToList();

        bool[] finishedTasks = await Task.WhenAll(tasks);
        return finishedTasks.ToList().All(result => result == true);
    }

    private List<MeshData> GetMeshesData(List<Part> parts)
    {
        return parts.Select(p => new MeshData(p.tissue.rusName, p.filePath, $"{researchDirPath}/{p.tissue.name}", new Threshold(1, 100))).ToList();
    }

    public void OnCellClicked(DataGridViewEventArgs args)
    {
        this.args = args;
    }

    void OnSeriesChanged(int seriesId)
    {
        seriesController.seriesChanged.RemoveListener(OnSeriesChanged);

        if (meshController.meshes.Count > 0)
        {
            RemoveMeshes();
        }

        imagesLoader.ClearViews();
        seriesController.RemoveSeries();
        mFController.RemoveElements();

        int id = seriesController.series[seriesId].id;
        LoadResearch();
        //LoadMeshes(seriesController.series[id]);
    }

    bool LoadMeshes(List<Part> parts)
    {
        if (MeshFilesExists(parts))
        {
            for (int j = 0; j < meshesData.Count; j++)
            {
                Part part = parts[j];
                MeshData data = meshesData[j];
                GameObject go = new OBJLoader().Load($"{part.meshFilePath}");
                data.gameObject = go;
                meshController.filters.Add(go.GetComponentInChildren<MeshFilter>());
                meshController.meshes.Add(go);
                go.transform.SetParent(meshController.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                Material material = go.GetComponentInChildren<MeshRenderer>().material;
                Color32 color = part.tissue.color;
                material.color = color;

                MeshFilterElement MFE = mFController.AddElement(data.Name, color);

                MFE.toggle.onValueChanged.AddListener(value => go.SetActive(value));
                MFE.colorChanged.AddListener(color => material.color = color);
            }

            meshController.Center();
            meshController.Resize(meshController.size);
            meshController.Rotate(meshController.rotation);
            meshController.Optimize();

            return true;
        }

        return false;

        bool MeshFilesExists(List<Part> parts)
        {
            return parts.Select(p => File.Exists(p.meshFilePath)).All(p => p == true);
        }
    }

    private async Task<bool> GenerateMeshes(List<MeshData> meshDatas)
    {
        int i = 0;

        List<Task> tasks = meshDatas.Select((meshData) =>
        {
            ServerParams serverParams = new ServerParams() { port = (uint)(80 + i) };
            Slicer3D slicer = new Slicer3D(slicerPath);
            Task task = slicer.GenerateMesh(meshData, serverParams);
            i++;

            return task;
        }).ToList();

        Task result = Task.WhenAll(tasks);

        try
        {
            await result;
        }
        catch(Exception e) {
            print(e.Message);
            return false;
        }

        return result.Status == TaskStatus.RanToCompletion ? true : false;
    }

    private async Task<Texture2D[]> GetThumbnails(Research research)
    {
        List<Task<Texture2D>> thumbnailTasks = research.series.Select(async s =>
        {
            string photosDirPath = $"{outputPath}/research_{research.id}_series_{s.id}/images/axial";
            if (Directory.Exists(photosDirPath))
            {
                string[] photoPaths = Directory.GetFiles(photosDirPath);
                int thumbnailId = photoPaths.Length > 0 ? Mathf.RoundToInt(photoPaths.Length / 2) : 0;

                return await imagesLoader.LoadImage(photoPaths[thumbnailId]);
            }

            return null;
        }).ToList();

        return await Task.WhenAll(thumbnailTasks);
    }

    private List<int> GetPhotosCount(Research research)
    {
        List<int> photosCount = research.series.Select(s =>
        {
            string photosDirPath = $"{outputPath}/research_{research.id}_series_{s.id}/images/axial";
            if (Directory.Exists(photosDirPath))
            {
                return Directory.GetFiles(photosDirPath).Length;
            }

            return 0;
        }).ToList();

        return photosCount;
    }

    void RemoveMeshes()
    {
        meshController.RemoveMeshes();
        meshes.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void OnResearchLoaded()
    {
        Logger.GetInstance().Success("Исследование успешно загружено");
        canvasController.SelectCanvas(3);
        Destroy(progressWindow.gameObject);
        progressWindow = null;
    }

    private void OnReasearchLoadFailed()
    {
        Destroy(progressWindow.gameObject);
        Logger.GetInstance().Error("При загрузке исследования произошла ошибка");
        canvasController.SelectCanvas(2);
        progressWindow = null;
    }

}