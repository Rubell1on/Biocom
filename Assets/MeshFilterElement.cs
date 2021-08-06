using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MeshFilterElement : MonoBehaviour
{
    public Text header;
    public Toggle toggle;
    public Slider r;
    public Text rText;
    public Slider g;
    public Text gText;
    public Slider b;
    public Text bText;
    public Slider a;
    public Text aText;
    public RawImage icon;
    public Toggle headerWrapper;

    public UnityEvent<Color32> colorChanged = new UnityEvent<Color32>();

    private void Start()
    {
        r.onValueChanged.AddListener(OnColorChanged);
        g.onValueChanged.AddListener(OnColorChanged);
        b.onValueChanged.AddListener(OnColorChanged);
        a.onValueChanged.AddListener(OnColorChanged);
    }

    public void SetHeader(string text)
    {
        header.text = text;
    }

    public void SetColor(Color32 color)
    {
        r.value = color.r;
        rText.text = color.r.ToString();
        g.value = color.g;
        gText.text = color.g.ToString();
        b.value = color.b;
        bText.text = color.b.ToString();
        a.value = color.a;
        aText.text = color.a.ToString();
        SetIconColor(color);
    }

    private void OnColorChanged(float f)
    {
        Color32 color = new Color32((byte)r.value, (byte)g.value, (byte)b.value, (byte)a.value);
        SetColor(color);

        colorChanged.Invoke(color);
    }

    private void SetIconColor(Color32 color)
    {
        icon.color = color;
    }

    private void OnDestroy()
    {
        r.onValueChanged.RemoveListener(OnColorChanged);
        g.onValueChanged.RemoveListener(OnColorChanged);
        b.onValueChanged.RemoveListener(OnColorChanged);
        a.onValueChanged.RemoveListener(OnColorChanged);
    }
}
