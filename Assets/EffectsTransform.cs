using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsTransform : MonoBehaviour
{
    public GameObject ui;


    public void Launch()
    {
        Instantiate(ui, transform);
    }
}
