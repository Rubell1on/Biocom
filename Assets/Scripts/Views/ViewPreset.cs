using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewPreset
{
    public string name;
    public List<Vector2> views;

    public ViewPreset(string name, List<Vector2> views)
    {
        this.name = name;
        this.views = views;
    }
}