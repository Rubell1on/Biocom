using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagesShutter : MonoBehaviour
{
    public RawImage left;
    public RawImage right;
    public bool even = true;

    private void Start()
    {
        Application.targetFrameRate = 48;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        left.gameObject.SetActive(even);
        right.gameObject.SetActive(!even);

        even = even ? false : true;
    }
}
