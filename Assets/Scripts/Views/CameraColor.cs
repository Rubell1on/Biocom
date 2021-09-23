using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    public ColorPallete colorPallete;
    public void OnColorChanged(Color color)
    {
        Camera.main.backgroundColor = color;
    }
    void Start()
    {
        colorPallete.TargetColor = Camera.main.backgroundColor;
    }

}
