using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CacheDirPath : MonoBehaviour
{
    public InputField inputField;
    public Button save;

    private string key = "CachePath";
    public void Start()
    {
        save.onClick.AddListener(SetPath);
        save.onClick.AddListener(CheckDirectory);
        inputField.text = PlayerPrefs.GetString(key);
    }

    private void SetPath()
    {
        PlayerPrefs.SetString(key, inputField.text);
    }

    private void CheckDirectory()
    {
        string path = PlayerPrefs.GetString(key);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    private void OnDestroy()
    {
        save.onClick.RemoveListener(SetPath);
        save.onClick.RemoveListener(CheckDirectory);
    }


}
