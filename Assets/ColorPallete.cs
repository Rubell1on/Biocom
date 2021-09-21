using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPallete : MonoBehaviour
{
    public Image image;
    public Camera camera;

    public void Click()
    {
        
        //Debug.Log(Input.mousePosition.ToString());
        //Debug.Log(color.ToString());
        Vector2 targetMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, Input.mousePosition, camera, out targetMousePos);

        targetMousePos.x -= image.rectTransform.rect.x;
        targetMousePos.y = targetMousePos.y - image.rectTransform.rect.y;
        //int x = (Screen.width - (int)image.rectTransform.rect.width - (int)Input.mousePosition.x) * -1;
        //int y = Screen.height - (int)image.rectTransform.rect.height - (int)Input.mousePosition.y;
        //Debug.Log(image.rectTransform.rect.Contains(Input.mousePosition));
        Color color = GetColorPallete((int)targetMousePos.x, (int)targetMousePos.y);
        camera.backgroundColor = color;
        Debug.Log(targetMousePos.ToString());

    }
    private Color GetColorPallete(int x, int y)
    {
        Texture2D texture = image.sprite.texture;
        return texture.GetPixel(x, y);
    }
}
