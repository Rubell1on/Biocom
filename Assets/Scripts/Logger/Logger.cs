using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WraithavenGames.UnityInterfaceSupport;

public class Logger : Singleton<Logger>, ILogger
{
    [SerializeField, InterfaceType(typeof(ILogger))]
    public List<MonoBehaviour> loggers;

    public void Log(string text)
    {
        foreach (ILogger l in loggers)
           l.Log(text);
    }

    public void Warning(string text)
    {
        foreach (ILogger l in loggers)
            l.Warning(text);
    }

    public void Error(string text)
    {
        foreach (ILogger l in loggers)
            l.Error(text);
    }
}