using UnityEngine;
using UnityEngine.Events;

public class HorizontalSizeScaler : MonoBehaviour
{
    public RectTransform wrapperRT;
    public float minWidth = 50;
    public UnityEvent<Vector2> scaleChanged = new UnityEvent<Vector2>();

    private Vector2 currMousePos;

    private void OnMouseEnter()
    {
        currMousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector2 mouseDelta = (Vector2)Input.mousePosition - currMousePos;

        if (mouseDelta.x != 0)
        {
            if (wrapperRT.sizeDelta.x + mouseDelta.x <= minWidth)
            {
                return;
            }
            else
            {
                wrapperRT.sizeDelta = new Vector2(wrapperRT.sizeDelta.x + mouseDelta.x, wrapperRT.sizeDelta.y);
            }

            scaleChanged.Invoke(wrapperRT.sizeDelta);
        }

        currMousePos = Input.mousePosition;
    }
}
