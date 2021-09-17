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

    public static string keyCache = "CachePath";

    public void Start()
    {
        save.onClick.AddListener(SetPath);
        screenSettings.onValueChanged.AddListener(ChangeWindowProgramm);

        FullScreenMode s = Screen.fullScreenMode;
        screenSettings.value = (int)s == 1 ? 0 : 1;

        CheckRegistry();
        inputField.text = PlayerPrefs.GetString(keyCache);
    }

    private void SetPath()
    {
        PlayerPrefs.SetString(keyCache, inputField.text);
    }

    private void CheckRegistry()
    {
        if (!PlayerPrefs.HasKey(keyCache) || PlayerPrefs.GetString(keyCache) == "")
        {
            PlayerPrefs.SetString(keyCache, $"{Application.dataPath}/tmp");
        }
    }

    private void OnDestroy()
    {
        save.onClick.RemoveListener(SetPath);
        screenSettings.onValueChanged.RemoveListener(ChangeWindowProgramm);
    }

    private void ChangeWindowProgramm(int i)
    {
        if (i == 0)
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
        if(i == 1)
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
    }

    public static string GetCachePath()
    {
        if (PlayerPrefs.HasKey(keyCache))
            return PlayerPrefs.GetString(keyCache);
        else
        {
            string cachePath = $"{ Application.dataPath}/tmp";
            PlayerPrefs.SetString(keyCache, cachePath);
            return cachePath;
        }
    }


}
