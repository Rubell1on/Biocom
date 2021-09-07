using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class Enviroments : MonoBehaviour
{
    public List<Enviroment> enviroments;
    public Button save;

    public void Start()
    {
        save.onClick.AddListener(SaveSettings);
        enviroments.ForEach(e => {
            CheckProgramAvailability(e, e.inputField.text);
        });
    }

    private void SaveSettings()
    {
        List<string> listEnvironment = GetListString();

        enviroments.ForEach(e =>
        {
            if (!PlayerPrefs.HasKey(e.key))
                PlayerPrefs.SetString(e.key, e.defaultPath);

            string oldPath = PlayerPrefs.GetString(e.key);
            string newPath = e.inputField.text;

            if (oldPath != newPath)
            {
                if (!listEnvironment.Contains(oldPath))
                    listEnvironment.Add(oldPath);
                listEnvironment = SetRecord(oldPath, newPath, listEnvironment);

                CheckProgramAvailability(e, newPath);
                PlayerPrefs.SetString(e.key, e.inputField.text);
            }

        });
        Environment.SetEnvironmentVariable("Path", GetEnviromentVariables(listEnvironment), EnvironmentVariableTarget.User);
    }

    private void CheckProgramAvailability(Enviroment e, string newPath)
    {
        if (CheckProgramm(newPath, e.programmName))
            UpdateUI(e.programmName, true, e.text);
        else
            UpdateUI(e.programmName, false, e.text);
    }

    private List<string> GetListString()
    {
        string enviromentVar = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
        return enviromentVar.Split(';').ToList();
    }
    private List<string> SetRecord(string oldPath, string newPath, List<string> list)
    {
        int id;
        id = list.FindIndex(str => str == oldPath);
        list[id] = newPath;
        return list;
    }

    private string GetEnviromentVariables(List<string> list)
    {
        return String.Join(";", list.Where(l => l != ""));
    }

    private bool CheckProgramm(string programmDir, string programmName)
    {
        if (Directory.Exists(programmDir))
            if (File.Exists(programmDir + "/" + programmName))
            {
                return true;
            }
        return false;
    }
    private void UpdateUI(string programmName, bool state, Text text)
    {
        if (state)
        {
            text.text = $"Файл {programmName} найден!";
            text.color = Color.green;
        }
        else
        {
            text.text = $"Файл {programmName} не найден!";
            text.color = Color.red;
        }
    }

    private void OnDestroy()
    {
        save.onClick.RemoveListener(SaveSettings);
    }

}