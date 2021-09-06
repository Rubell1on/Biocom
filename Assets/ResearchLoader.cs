using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CatchyClick;
using Dummiesman;

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

    public async void LoadResearch()
    {
        DataGridViewRow row = dataGrid.rows[args.row];
        int index;
        seriesController.RemoveSeries();

        if (Int32.TryParse(row.cells[0].value, out index))
        {
            GameObject modalWindowInstance = Instantiate(progressWindowTemplate.gameObject, mainCanvas);
            if (modalWindowInstance)
            {
                progressWindow = modalWindowInstance.GetComponent<LoadingModalWindow>();
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            researchDirPath = $"{outputPath}/{index}";

            if (!Directory.Exists(researchDirPath))
            {
                Directory.CreateDirectory(researchDirPath);
            }

            ////Images
            //Research research = DBResearches.GetReasearchById(index);

            //progressWindow.bodyText.text = $"Идет процесс получения снимков КТ из файла {research.sourceNiiFilePath}";

            //await NiiImagesExporter.Run(research.sourceNiiFilePath, researchDirPath);

            //progressWindow.bodyText.text = $"Идет процесс загрузки снимков КТ";

            //List<string> directories = Directory.GetDirectories($"{researchDirPath}/images").ToList();

            //if (directories.Count > 0)
            //{
            //    Texture2D[][] textures = await imagesLoader.GetImagesFromDirectories(directories);

            //    for (int i = 0; i < directories.Count; i++)
            //    {
            //        imagesLoader.viewers[i].SetImages(textures[i].ToList());
            //    }
            //}

            //Parts

            progressWindow.bodyText.text = $"Идет процесс генерации мешей";

            List<Part> parts = DBParts.GetPartsByResearchId(index);
            if (parts.Count > 0)
            {
                Dictionary<int, List<Part>> series = Part.GetSeries(parts);
                int seriesId = series.Keys.ToArray()[seriesController.current];

                List<Part> singleSeries = series[seriesId];
                meshesData = GetMeshesData(singleSeries);

                bool meshesExists = singleSeries
                    .Select(p => File.Exists(p.meshFilePath))
                    .All(e => e == true);

                if (!meshesExists)
                {
                    bool generationFinished = await GenerateMeshes(meshesData);

                    if (generationFinished)
                    {
                        bool partsUpdated = UpdatePartsData(singleSeries);

                        if (partsUpdated)
                        {
                            parts = DBParts.GetPartsByResearchId(index);
                            series = Part.GetSeries(parts);
                            seriesId = series.Keys.ToArray()[seriesController.current];
                        }
                    }
                }

                bool meshesLoaded = LoadMeshes(series[seriesId]);

                if (meshesLoaded)
                {
                    seriesController.AddSeriesRange(series);
                    seriesController.seriesChanged.AddListener(OnSeriesChanged);
                    return;
                }
                else
                {

                }
            }
            else
            {

            }
        }
        else
        {

        }

        researchLoadFailed.Invoke();
    }

    private bool UpdatePartsData(List<Part> parts)
    {
        return meshesData.Select(m =>
        {
            int id = parts.Find(p => p.filePath == m.inputFilePath).id;
            Dictionary<string, string> dictionary = new Dictionary<string, string>() { { "meshFilePath", $"{m.outputFilePath}/Segmentation.obj" } };
            QueryBuilder queryBuilder = new QueryBuilder(dictionary);
            return DBParts.EditPart(id, queryBuilder);
        }).All(result => result == true);
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
        if (meshController.meshes.Count > 0)
        {
            RemoveMeshes();
        }

        imagesLoader.ClearViews();

        mFController.RemoveElements();
        int id = seriesController.series.Keys.ToArray()[seriesId];
        LoadMeshes(seriesController.series[id]);
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

            Debug.Log("Finished");
            researchLoaded.Invoke();

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

    void RemoveMeshes()
    {
        meshController.RemoveMeshes();
        meshes.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void OnResearchLoaded()
    {
        Logger.GetInstance().Log("Исследование успешно загружено");
        canvasController.SelectCanvas(3);
        Destroy(progressWindow.gameObject);
    }

    private void OnReasearchLoadFailed()
    {
        Logger.GetInstance().Error("При загрузке исследования произошла ошибка");
        canvasController.SelectCanvas(2);
    }
}