using System.Threading.Tasks;
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

    private static readonly string key = "keyConnectionString";

    public static async Task<MySqlConnection> GetConnection()
    {
        connectionString = GetConnectionString();
        connectionData = ConnectionData.Parse(connectionString);

        try
        {
            return await Task.Run<MySqlConnection>(async () =>
            {
                connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                return connection;
            });
            //connection = new MySqlConnection(connectionString);
            //await connection.OpenAsync();
            //return connection;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("��������� �����������: " + connection.State);
            Logger.GetInstance().Error("������ ����������� ���� ������: " + e);
            return null;
        }
    }

    public static string GetConnectionString()
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetConnectionString(string connectionString)
    {
        PlayerPrefs.SetString(key, connectionString);
    }

    public static bool ConnectionStringExists()
    {
        return PlayerPrefs.HasKey(key);
    }
}