using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CudeRotation : MonoBehaviour
{
    public float amplifier = 1;
    public float value = 0;
    // Update is called once per frame
    void Update()
    {
        value += amplifier * Time.deltaTime;
        gameObject.transform.localRotation = Quaternion.Euler(-33f, value, 0f);
    }
}
