using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ImageViewer : MonoBehaviour
{
    public RawImage image;
    public Scrollbar scrollbar;
    [HideInInspector]
    public int id = -1;

    public ValueChanged valueChanged = new ValueChanged();

    private List<Texture2D> textures;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener(OnScrollChanged);
        valueChanged.AddListener(OnValueChanged);
    }

    public void SetImages(List<Texture2D> textures)
    {
        this.textures = textures;
        scrollbar.numberOfSteps = textures.Count;
        SelectImage(0);
    }

    public void SelectImage(int id)
    {
        if (textures.Count > 0)
        {
            image.texture = textures[id];
        }
    }

    private void OnScrollChanged(float value)
    {
        double rounded = Math.Floor(value * scrollbar.numberOfSteps);
        int id = Convert.ToInt32(value == 1 ? rounded - 1: rounded);
        if (this.id != id)
        {
            valueChanged.Invoke(id);
            this.id = id;
        }
    }

    private void OnValueChanged(int id)
    {
        SelectImage(id);
    }
}

public class ValueChanged : UnityEvent<int> { }