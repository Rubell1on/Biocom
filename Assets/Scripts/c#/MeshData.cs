using System.Collections.Generic;
using UnityEngine;
public class MeshData
{
    public string name;
    public string inputFilePath;
    public string outputFilePath;
    public GameObject gameObject;

    public Threshold threshold;

    public string Name
    {
        get
        {
            return translation.ContainsKey(name) ? translation[name] : name;
        }
    }

    private Dictionary<string, string> translation = new Dictionary<string, string>()
    {
        { "lungs", "Легкие" },
        { "vessels", "Бронхи" },
        { "trahea", "Трахея" },
        { "mat", "Матовое стекло"}
    };

    public MeshData(string name, string input, string output, Threshold threshold)
    {
        this.name = name;
        this.inputFilePath = input;
        this.outputFilePath = output;
        this.threshold = threshold;
    }
}
