using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewColorController : MonoBehaviour
{
    public GameObject panel;
    public ViewsController viewsController;

    private ColorPallete colorPallete;
    public void CreateColorPalette()
    {
        if (colorPallete == null)
        {
            colorPallete = Instantiate(panel, new Vector2(20, 20), Quaternion.identity, transform.parent).GetComponent<ColorPallete>();
            colorPallete.colorChanged.AddListener(OnColorChanged);
            colorPallete.TargetColor = viewsController.backgroundColor;
        }
    }

    private void OnColorChanged(Color color)
    {
        viewsController.backgroundColor = color;
        viewsController.viewChanged.Invoke();
    }
}
