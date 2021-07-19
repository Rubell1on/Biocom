using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;

public class MeshesLoader : MonoBehaviour
{
    List<string> paths = new List<string>()
    {
        "E:/tmp/lungs/Segmentation.obj",
        "E:/tmp/trahea/Segmentation.obj",
        "E:/tmp/vessels/Segmentation.obj",
    };

    public MeshController meshController;
    public Material material;

    // Start is called before the first frame update
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

        meshController.Resize(meshController.size);
        meshController.Center();

        transform.localRotation = Quaternion.Euler(meshController.rotation);
    }
}
