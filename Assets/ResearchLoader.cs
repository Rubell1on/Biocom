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
    public string outputPath = "E:/tmp";
    public DataGridView dataGrid;

    public UnityEvent meshesLoaded = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        dataGrid = GetComponent<DataGridView>();
        dataGrid.cellClicked.AddListener(OnCellClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnCellClicked(DataGridViewEventArgs args)
    {
        DataGridViewRow row = dataGrid.rows[args.row];
        int index;

        if (Int32.TryParse(row.cells[0].value, out index)) {
            List<Part> parts = DBParts.GetPartsByResearchId(index);
            List<MeshData> meshDatas = parts.Select(p => new MeshData(p.partName, p.filePath, $"{outputPath}/{p.partName}", new Threshold(1, 100))).ToList();

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

            foreach (MeshData data in meshDatas)
            {
                GameObject go = new OBJLoader().Load($"{data.outputFilePath}/Segmentation.obj");
            }

            Debug.Log("Finished");
            meshesLoaded.Invoke();
        } else
        {

        }
    }
}