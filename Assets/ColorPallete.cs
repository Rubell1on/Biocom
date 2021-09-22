using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorPallete : MonoBehaviour
{
    public Image image;
    public Image targetColorImage;
    public RectTransform rect;
    public InputField hexInputField;

    public Vector2 sourceImageSize = new Vector2(500, 500);
    public Vector2 offset;
    private Color32 targetColor;
    public Color32 TargetColor
    {
        get { return targetColor; }
        set 
        {
            targetColorImage.color = value;
            hexInputField.text = "#" + ColorUtility.ToHtmlStringRGBA(value);
            targetColor = value;
            colorChanged.Invoke(value);
        }
    }
    public string Hex { get { return hexInputField.text; } }

    public PaletteColorChangedEvent colorChanged = new PaletteColorChangedEvent();

    public void GetColor()
    {
        float coefX = sourceImageSize.x / image.rectTransform.rect.size.x;
        float coefY = sourceImageSize.y / image.rectTransform.rect.size.y;

        float targetPosX = (Input.mousePosition.x - rect.offsetMin.x - offset.x) * coefX;
        float targetPosY = (Input.mousePosition.y - rect.offsetMin.y - offset.y) * coefY;

        Color color = GetColorPallete((int)(targetPosX), (int)(targetPosY));
        TargetColor = color;
    }

    private Color GetColorPallete(int x, int y)
    {
        Texture2D texture = image.sprite.texture;
        return texture.GetPixel(x, y);
    }

    public void OnHexFieldEndEdit(string field)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(field, out color))
            TargetColor = color;
        else
            Logger.GetInstance().Error("¬веден не правильный формат HEX кода.");
    }

}

[System.Serializable]
public class PaletteColorChangedEvent : UnityEvent<Color> { }
