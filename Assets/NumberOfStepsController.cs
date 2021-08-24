using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class NumberOfStepsController : MonoBehaviour
{
    public Scrollbar scrollbar;
    public InputField field;

    private UnityEvent creep;

    public void Start()
    {
        scrollbar.onValueChanged.AddListener(OnScrollChanged);
        field.onEndEdit.AddListener(ChangeValue);
    }
    public void ChangeValue(string str)
    {

        //Не могу понять как посчитать. Надо додумать.
        if (Convert.ToInt32(str) == 1)
            scrollbar.value = 0;
        else
        {
            float value = 1f / (scrollbar.numberOfSteps - 1) * Convert.ToInt32(str);
            scrollbar.value = value;
        }
    }

    private void OnScrollChanged(float value)
    {
        double rounded = Math.Floor(value * scrollbar.numberOfSteps);
        int id = Convert.ToInt32(value == 1 ? rounded - 1 : rounded);
        field.text = (id+1).ToString();
    }
}
