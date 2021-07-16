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
            new MeshData("lungs", "C:/Users/AKhok/Downloads/Результаты Елизаветов/Результаты Елизаветов/P29_lungs.nii", "E:/tmp/lungs", new Threshold(1, 100)),
            new MeshData("vessels", "C:/Users/AKhok/Downloads/Результаты Елизаветов/Результаты Елизаветов/P29_bronchi_vessels.nii", "E:/tmp/vessels", new Threshold(1, 100)),
            new MeshData("trahea", "C:/Users/AKhok/Downloads/Результаты Елизаветов/Результаты Елизаветов/P29_trahea.nii", "E:/tmp/trahea", new Threshold(1, 100)),
        };

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
        //Task.WaitAll(tasks);

        foreach (MeshData data in meshDatas)
        {
            GameObject go = new OBJLoader().Load($"{data.outputFilePath}/Segmentation.obj");
        }

        Debug.Log("Finished");
    }
}