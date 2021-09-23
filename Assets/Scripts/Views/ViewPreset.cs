using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewPreset
{
    public string name;
    public List<View> views;
    public float size;
    

    public ViewPreset(string name, List<View> views, float size = 10)
    {
        this.name = name;
        this.views = views;
        this.size = size;
    }

    public class View
    {
        public Vector2 gridPosition;
        public Vector3 cameraPosition;
        public Vector3 cameraRotaion;

        public View(Vector2 gridPosition, Vector3 cameraPosition, Vector3 cameraRotaion)
        {
            this.gridPosition = gridPosition;
            this.cameraPosition = cameraPosition;
            this.cameraRotaion = cameraRotaion;
        }
    }
}