using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class SQLConnection
{
    private static string connectionString;
    public static ConnectionData connectionData = null;
    private static MySqlConnection connection = null;

    public static MySqlConnection GetConnection()
    {
        if (connectionData == null)
        {
            connectionString = GetConnectionString();
            connectionData = ConnectionData.Parse(connectionString);
        }

        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Состояние подключение: " + connection.State);
            Logger.GetInstance().Error("Ошибка подключения базы данных: " + e);
            return null;
        }
    }

    public static string GetConnectionString()
    {

        return UnityEngine.Resources.Load<TextAsset>("connectionString").text;
    }
}