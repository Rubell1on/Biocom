using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderClick : MonoBehaviour
{
    public InputField inputField;
    public Button handle;
    DoubleClick<string> doubleClick = new DoubleClick<string>();
    void Start()
    {
        doubleClick.AddListener(s => OnHandleClick());
        handle.onClick.AddListener(() => doubleClick.Invoke(""));
    }

    void OnHandleClick()
    {
        if (!inputField.gameObject.activeSelf)
        {
            inputField.gameObject.SetActive(true);
            inputField.ActivateInputField();
        }
    }
}
