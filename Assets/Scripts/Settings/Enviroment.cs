using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enviroment : MonoBehaviour
{
    public string programmName;
    public InputField inputField;
    public Text text;
    public string defaultPath;
    public string key;

    private void Awake()
    {
        inputField.text = PlayerPrefs.GetString(key, defaultPath);
    }

}
