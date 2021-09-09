using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResourceExtractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> files = new List<string>() { 
            "scripts/python/GenerateMesh",
            "scripts/python/Segmenter",
            "scripts/python/slicerApp",
            "scripts/python/test",
            "scripts/python/SimpleITK/ExportImages",
            "scripts/python/SimpleITK/SimpleITKImages"
        };
        bool result = files.Select(f => ResourcesExtractor.ExtractTextAsset(f, "py")).All(e => e == true);
        Debug.Log("");
    }
}
