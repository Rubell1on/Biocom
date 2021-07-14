using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class SQLConnection
{
    private static string
        host = "127.0.0.1",
        database = "biocom",
        user = "admin",
        password = "1Lovelena";
    public static bool pooling = true;
    private static string connectionString;
    private static MySqlConnection connection = null;

    public static MySqlConnection GetConnection()
    {
        connectionString = $"Server={host};Database={database};User={user};{(password.Length > 0 ? "Password=" + password + ";" : "")}Pooling=";
        if (pooling)
            connectionString += "True";
        else
            connectionString += "False";

        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Состояние подключение: " + connection.State);
            return connection;
        }
        catch (Exception e)
        {
            Debug.Log("Состояние подключение:" + connection.State);
            Debug.Log("Ошибка подключения базы данных: " + e);
            return null;
        }
    }

}
