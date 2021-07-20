using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class PartsDataForm : MonoBehaviour
{
    public Button applyButton;
    public Text text;

    public InputField partName;
    public InputField partPath;
    public Dropdown seriesId;

    public void SetInfo(string buttonText, string labelText, List<string> series)
    {
        applyButton.GetComponentInChildren<Text>().text = buttonText;
        text.text = labelText;
        seriesId.ClearOptions();
        seriesId.AddOptions(series);
    }
}
