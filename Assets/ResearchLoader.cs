using System;
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
    public string outputPath = "E:/tmp";
    public DataGridView dataGrid;
    public RectTransform progressWindowTemplate;
    public MeshController meshController;
    public List<Material> meshMaterials;
    public GameObject meshes;
    public SeriesController seriesController;
    public List<MeshData> meshDatas;
    public MeshFilterController mFController;

    public UnityEvent meshesLoaded = new UnityEvent();

    private GameObject progressWindow;
    private DataGridViewEventArgs args;

    void Start()
    {
        dataGrid = GetComponent<DataGridView>();
        dataGrid.cellClicked.AddListener(OnCellClicked);
        meshesLoaded.AddListener(OnMeshLoaded);
    }
    public async void LoadResearch()
    {
        DataGridViewRow row = dataGrid.rows[args.row];
        int index;
        seriesController.RemoveSeries();

        if (Int32.TryParse(row.cells[0].value, out index))
        {
            progressWindow = Instantiate(progressWindowTemplate.gameObject, mainCanvas);

            List<Part> parts = DBParts.GetPartsByResearchId(index);
            if (parts.Count > 0)
            {
                Dictionary<int, List<Part>> series = Part.GetSeries(parts);
                int seriesId = series.Keys.ToArray()[seriesController.current];
                await LoadMeshes(series[seriesId]);
                seriesController.AddSeriesRange(series);
                seriesController.seriesChanged.AddListener(OnSeriesChanged);
            }
            else
            {

            }
        }
        else
        {

        }
    }
    public void OnCellClicked(DataGridViewEventArgs args)
    {
        this.args = args;
    }

    async void OnSeriesChanged(int seriesId)
    {
        if (meshController.meshes.Count > 0)
        {
            RemoveMeshes();
        }

        int id = seriesController.series.Keys.ToArray()[seriesId];
        await LoadMeshes(seriesController.series[id]);
    }

    async Task LoadMeshes(List<Part> parts)
    {
        mFController.RemoveElements();
        meshDatas = parts.Select(p => new MeshData(p.partName, p.filePath, $"{outputPath}/{p.partName}", new Threshold(1, 100))).ToList();
        int i = 0;

        List<Task> tasks = meshDatas.Select((meshData) =>
        {
            ServerParams serverParams = new ServerParams() { port = (uint)(80 + i) };
            Slicer3D slicer = new Slicer3D("E:/Programms/Slicer 4.11.20210226");
            Task task = slicer.GenerateMesh(meshData, serverParams);
            i++;

            return task;
        }).ToList();

        await Task.WhenAll(tasks);

        for (int j = 0; j < meshDatas.Count; j++)  
        {
            MeshData data = meshDatas[j];
            GameObject go = new OBJLoader().Load($"{data.outputFilePath}/Segmentation.obj");
            data.gameObject = go;
            meshController.filters.Add(go.GetComponentInChildren<MeshFilter>());
            meshController.meshes.Add(go);
            go.transform.SetParent(meshController.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            Material material = j < meshMaterials.Count ? meshMaterials[j] : meshMaterials[0];
            go.GetComponentInChildren<MeshRenderer>().material = material;

            MeshFilterElement MFE = mFController.AddElement(data.name, material.color);

            MFE.toggle.onValueChanged.AddListener(value => go.SetActive(value));
            MFE.colorChanged.AddListener(color => material.color = color);
        }

        meshController.Center();
        meshController.Resize(meshController.size);
        //meshController.Rotate(meshController.rotation);
        meshController.Optimize();

        meshes.transform.localRotation = Quaternion.Euler(meshController.rotation);

        Debug.Log("Finished");
        meshesLoaded.Invoke();

        return;
    }

    void RemoveMeshes()
    {
        meshController.RemoveMeshes();
        meshes.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void OnMeshLoaded()
    {
        Destroy(progressWindow);
    }
}