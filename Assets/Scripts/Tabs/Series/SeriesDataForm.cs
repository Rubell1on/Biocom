using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SeriesDataForm : MonoBehaviour
{
    public Button applyButton;
    public Text text;

    public InputField seriesName;
    public InputField description;
    public InputField filePath;
    public Dropdown researchId;

    public void SetInfo(string buttonText, string labelText)
    {
        applyButton.GetComponentInChildren<Text>().text = buttonText;
        text.text = labelText;
    }
}
