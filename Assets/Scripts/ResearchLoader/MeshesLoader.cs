using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;

public class MeshesLoader : MonoBehaviour
{
    public string dirPath = "C:/tmp";
    List<string> paths;

    public MeshController meshController;
    public Material material;

    private void Awake()
    {
        paths = new List<string>()
        {
            $"{dirPath}/lungs/Segmentation.obj",
            $"{dirPath}/trahea/Segmentation.obj",
            $"{dirPath}/vessels/Segmentation.obj",
        };
    }
    void Start()
    {
        foreach (string path in paths)
        {
            GameObject go = new OBJLoader().Load(path);
            meshController.filters.Add(go.GetComponentInChildren<MeshFilter>());
            go.transform.SetParent(meshController.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.GetComponentInChildren<MeshRenderer>().material = material;
        }

        meshController.Center();
        meshController.Resize(meshController.size);
        meshController.Rotate(meshController.rotation);
    }
}
