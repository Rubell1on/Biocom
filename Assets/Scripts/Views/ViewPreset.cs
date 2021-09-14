using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewPreset
{
    public string name;
    public List<Vector2> views;
    public float size;

    public ViewPreset(string name, List<Vector2> views, float size = 10)
    {
        this.name = name;
        this.views = views;
        this.size = size;
    }
}