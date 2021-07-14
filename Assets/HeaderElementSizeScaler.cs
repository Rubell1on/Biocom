using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderElementSizeScaler : MonoBehaviour
{
    public RectTransform wrapperRT;
    public float minWidth = 50;

    private Vector2 currMousePos;

    private void OnMouseEnter()
    {
        currMousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector2 mouseDelta = (Vector2)Input.mousePosition - currMousePos;
        if (wrapperRT.sizeDelta.x + mouseDelta.x <= minWidth)
        {
            return;
        } else
        {
            wrapperRT.sizeDelta = new Vector2(wrapperRT.sizeDelta.x + mouseDelta.x, wrapperRT.sizeDelta.y);
        }

        currMousePos = Input.mousePosition;
    }
}
