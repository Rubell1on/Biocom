using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UserDataForm : MonoBehaviour
{
    public Button applyButton;
    public Text text;

    public InputField username;
    public InputField password;
    public Dropdown role;
 

    public void SetInfo(string buttonText, string labelText)
    {
        applyButton.GetComponentInChildren<Text>().text = buttonText;
        text.text = labelText;

        role.ClearOptions();
        role.AddOptions(new List<string>(Enum.GetNames(typeof(User.Role))));
    }
}
