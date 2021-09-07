using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class PythonExec : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await NiiImagesExporter.Export("C:/tmp/P029_source.nii", "C:/tmp");
        UnityEngine.Debug.Log("Done");
    }
}