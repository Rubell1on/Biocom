using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshFilterController : MonoBehaviour
{
    public GameObject template;
    public GameObject body;
    public ToggleGroup toggleGroup;

    private List<MeshFilterElement> elements = new List<MeshFilterElement>();
    
    public MeshFilterElement AddElement()
    {
        GameObject instance = Instantiate(template, body.transform);
        MeshFilterElement element = instance.GetComponent<MeshFilterElement>();

        elements.Add(element);
        return element;
    }
    public MeshFilterElement AddElement(string header, Color32 color)
    {
        GameObject instance = Instantiate(template, body.transform);
        MeshFilterElement element = instance.GetComponent<MeshFilterElement>();
        element.headerWrapper.group = toggleGroup;
        element.SetHeader(header);
        element.SetColor(color);

        elements.Add(element);

        return element;
    }

    public void RemoveElements()
    {
        elements.ForEach(p => Destroy(p));
    }

    //private void OnDisable()
    //{
    //    RemoveElements();
    //}
}
