using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ResearchDataForm : MonoBehaviour
{
    public Button applyButton;
    public Text text;

    public InputField description;
    public InputField note;
    public Dropdown state;
    public Dropdown userName;

    public void SetInfo(string buttonText, string labelText, List<string> userNames, string description = "", string note = "")
    {
        applyButton.GetComponentInChildren<Text>().text = buttonText;
        text.text = labelText;

        this.description.text = description;
        this.note.text = note;
        state.ClearOptions();
        userName.ClearOptions();
        state.AddOptions(new List<string>(Enum.GetNames(typeof(Research.State))));
        userName.AddOptions(userNames);
    }
}
