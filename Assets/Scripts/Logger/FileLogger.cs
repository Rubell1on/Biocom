using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileLogger : Singleton<FileLogger>, ILogger
{
    public string dirPath;
    public enum LogType {Log, Success, Warning, Error};
    private Dictionary<LogType, string> dictionary = new Dictionary<LogType, string>() 
    {
        {LogType.Log, "log"},
        {LogType.Success, "success"},
        {LogType.Warning, "warning"},
        {LogType.Error, "error"}
    };

    private void Start()
    {
        dirPath = Application.dataPath + "/Logs";
    }

    public void Log(string text)
    {
        _Log(text, LogType.Log);
    }

    public void Success(string text)
    {
        _Log(text, LogType.Success);
    }
    public void Warning(string text)
    {
        _Log(text, LogType.Warning);
    }
    public void Error(string text)
    {
        _Log(text, LogType.Error);
    }

    private async void _Log(string text, LogType type)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string fileName = DateTime.Now.ToString("yyyy-MM-dd") + $"_{dictionary[type].ToString()}.txt";
        string fullPath = dirPath + "/" + fileName;

        if (!File.Exists(fullPath))
            File.Create(fullPath).Close();

        using (StreamWriter writer = File.AppendText(fullPath))
        {
            await writer.WriteAsync($"[{DateTime.Now.ToString("HH:mm:ss")}] - " + text + "\n");
        }
    }
}
