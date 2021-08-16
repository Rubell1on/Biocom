using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.type = Image.Type.Filled;
        image.fillAmount = 0f;
    }

    private void Sliced()
    {
        //image.
    }
}
