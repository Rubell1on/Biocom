using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Dummiesman;

public class ServerController : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        //Slicer3D slicer = new Slicer3D("H:/Program Files/Slicer 4.11.20210226");

        List<MeshData> meshDatas = new List<MeshData>()
        {
            new MeshData("lungs", "H:/HDD Download/results Elizavetov/results Elizavetov/P29_lungs.nii", "C:/tmp/lungs", new Threshold()),
            new MeshData("vessels", "H:/HDD Download/results Elizavetov/results Elizavetov/P29_bronchi_vessels.nii", "C:/tmp/vessels", new Threshold(1, 4)),
            new MeshData("trahea", "H:/HDD Download/results Elizavetov/results Elizavetov/P29_trahea.nii", "C:/tmp/trahea", new Threshold(1, 2)),
        };

        int i = 0;

        List<Task> tasks = meshDatas.Select((meshData) =>
        {
            ServerParams serverParams = new ServerParams() { port = (uint)(80 + i) };
            Slicer3D slicer = new Slicer3D("H:/Program Files/Slicer 4.11.20210226");
            Task task = slicer.GenerateMesh(meshData, serverParams);
            i++;

            return task;
        }).ToList();

        await Task.WhenAll(tasks);
        //Task.WaitAll(tasks);

        foreach (MeshData data in meshDatas)
        {
            GameObject go = new OBJLoader().Load($"{data.outputFilePath}/Segmentation.obj");
        }

        Debug.Log("Finished");
    }
}