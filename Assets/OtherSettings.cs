using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OtherSettings : MonoBehaviour
{
    public InputField inputField;
    public Button save;
    public Dropdown screenSettings;

    private const string keyCashe = "CachePath";

    public void Start()
    {
        save.onClick.AddListener(SetPath);
        save.onClick.AddListener(CheckDirectory);
        screenSettings.onValueChanged.AddListener(ChangeWindowProgramm);

        FullScreenMode s = Screen.fullScreenMode;
        screenSettings.value = (int)s == 1 ? 0 : 1;

        CheckRegistry();
        inputField.text = PlayerPrefs.GetString(keyCashe);
    }

    private void SetPath()
    {
        PlayerPrefs.SetString(keyCashe, inputField.text);
    }

    private void CheckRegistry()
    {
        if (!PlayerPrefs.HasKey(keyCashe) || PlayerPrefs.GetString(keyCashe) == "")
        {
            PlayerPrefs.SetString(keyCashe, $"{Application.dataPath}/tmp");
            CheckDirectory();
        }
    }

    private void CheckDirectory()
    {
        string path = PlayerPrefs.GetString(keyCashe);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        Logger.GetInstance().Success("Папка для кеш файлов сохранена.");
    }
    private void OnDestroy()
    {
        save.onClick.RemoveListener(SetPath);
        save.onClick.RemoveListener(CheckDirectory);
        screenSettings.onValueChanged.RemoveListener(ChangeWindowProgramm);
    }

    private void ChangeWindowProgramm(int i)
    {
        if (i == 0)
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
        if(i == 1)
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
    }




}
