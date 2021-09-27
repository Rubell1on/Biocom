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
    public Slider slider;
    public InputField field;
    public Button button;
    [HideInInspector]
    public int id = -1;

    public ValueChanged valueChanged = new ValueChanged();

    private List<Texture2D> textures = new List<Texture2D>();
    private DoubleClick<string> doubleClick = new DoubleClick<string>();

    private void Start()
    {   
        slider.minValue = 1;
        field.text = slider.value.ToString();
        slider.onValueChanged.AddListener(OnScrollChanged);
        valueChanged.AddListener(OnValueChanged);
        doubleClick.AddListener(d => OnHandleClick());
        button.onClick.AddListener(() => doubleClick.Invoke(""));
    }

    public void SetImages(List<Texture2D> textures)
    {
        this.textures = textures;
        slider.maxValue = textures.Count;
        SelectImage(0);
    }

    public void RemoveImages()
    {
        slider.maxValue = 1;
        textures.Clear();
        image.texture = null;
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
        int id = Convert.ToInt32(value);
        if (this.id != id)
        {
            field.text = id.ToString();
            valueChanged.Invoke(id - 1);
            this.id = id;
        }
    }

    public void ChangeShort()
    {
        if (Convert.ToInt32(field.text) > slider.maxValue)
            field.text = slider.maxValue.ToString();
        if (Convert.ToInt32(field.text) < 1)
            field.text = "1";

        OnScrollChanged(Convert.ToUInt32(field.text));
        slider.value = Convert.ToUInt32(field.text);
    }

    private void OnValueChanged(int id)
    {
        SelectImage(id);
    }

    private void OnHandleClick()
    {
        if (!field.gameObject.activeSelf)
        {
            field.gameObject.SetActive(true);
            field.ActivateInputField();
        }
    }
}

public class ValueChanged : UnityEvent<int> { }