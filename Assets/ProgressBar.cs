using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public RectTransform wrapper;
    public RectTransform bar;
    public bool infinite = false;
    [Range(0f, 1f)]
    public float defaultWidthMultiplyer = 0.8f;
    public float speedMultiplyer = 10;
    [Range(0f, 1f)]
    public float value = 0;
    private float oldValue = 0;

    private void Start()
    {
        oldValue = value;

        if (infinite)
        {
            StartInfiniteMove();
        } else
        {
            SetWidth(0);
        }
    }

    public void Update()
    {
        if (value != oldValue)
        {
            if (!InRange(value)) 
            {
                value = value < 0 ? 0 : 1;
            }

            float wrapperWidth = wrapper.rect.width;
            SetWidth(wrapperWidth * this.value);

            oldValue = value;
        }
    }

    private bool InRange(float value)
    {
        return value < 0 || value > 1 ? false : true;
    }

    public void SetWidth(float value)
    {
        bar.sizeDelta = new Vector2(value, bar.sizeDelta.y);
    }


    private void StartInfiniteMove()
    {
        float width = wrapper.rect.width * defaultWidthMultiplyer;
        SetWidth(width);
        bar.anchoredPosition = new Vector2(-width, 0);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while(bar.anchoredPosition.x < wrapper.rect.width)
        {
            bar.anchoredPosition = new Vector2(bar.anchoredPosition.x + speedMultiplyer * Time.deltaTime, 0);
            Debug.Log(bar.anchoredPosition.x);
            yield return new WaitForEndOfFrame();
        }

        StartInfiniteMove();
    }
}
