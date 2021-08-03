using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScrollSize : MonoBehaviour
{
    public int stepSizes;

    private Scrollbar scrollbar;
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.numberOfSteps = stepSizes;
    }
}
