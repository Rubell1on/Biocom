using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform gameObj;
    public RectTransform maskWrapper;
    public float speed = 1000f;
    public float spacing = 5f;
    public enum SlideDirection {None, Horizontal, Vertical}
    public SlideDirection slideDirection;

    private HorizontalLayoutGroup horizontalLayoutGroup;
    private VerticalLayoutGroup verticalLayoutGroup;
    private Coroutine coroutine;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Slide(false);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Slide(true);
    }

    public void Start()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }
    public void Slide(bool active = false)
    {
        Vector2 spacing = GetLayoutSpacing();

        if (coroutine == null)
            coroutine = StartCoroutine(_Slide(spacing, active));
        else 
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(_Slide(spacing, active));
        }
    }

    IEnumerator _Slide(Vector2 spacing, bool active = false)
    {
        if (!active)
        {
            Vector2 difference = new Vector2(gameObj.rect.width + spacing.x, gameObj.rect.height + spacing.y);

            while (maskWrapper.rect.width <= difference.x && maskWrapper.rect.height <= difference.y)
            {
                if(maskWrapper.rect.width < difference.x)
                    maskWrapper.sizeDelta = new Vector2(maskWrapper.sizeDelta.x + Time.deltaTime * speed, maskWrapper.sizeDelta.y);
                if (maskWrapper.rect.width < difference.y)
                    maskWrapper.sizeDelta = new Vector2(maskWrapper.sizeDelta.x, maskWrapper.sizeDelta.y + Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
            maskWrapper.sizeDelta = new Vector2(difference.x, difference.y);
        }
        else
        {

            while (maskWrapper.rect.width >= 0 && maskWrapper.rect.height >= 0)
            {
                //Vector2 difference = new Vector2(gameObj.rect.width + spacing.x, gameObj.rect.height + spacing.y);

                if (maskWrapper.rect.width > 0)
                    maskWrapper.sizeDelta = new Vector2(maskWrapper.sizeDelta.x - Time.deltaTime * speed, maskWrapper.sizeDelta.y);
                if (maskWrapper.rect.height > 0)
                    maskWrapper.sizeDelta = new Vector2(maskWrapper.sizeDelta.x, maskWrapper.sizeDelta.y - Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
            maskWrapper.sizeDelta = Vector2.zero;
            if(horizontalLayoutGroup != null)
                horizontalLayoutGroup.spacing = 0;
            if (verticalLayoutGroup != null)
                verticalLayoutGroup.spacing = 0;
        }
    }

    private Vector2 GetLayoutSpacing()
    {
        if (slideDirection == SlideDirection.None)
            return Vector2.zero;
        if (slideDirection == SlideDirection.Horizontal)
        {
            horizontalLayoutGroup.spacing = spacing;        
            return new Vector2(horizontalLayoutGroup.spacing, 0);
        }
        if (slideDirection == SlideDirection.Vertical)
        {
            verticalLayoutGroup.spacing = spacing;
            return new Vector2(0, verticalLayoutGroup.spacing);
        }
        return Vector2.zero;
    }
}
