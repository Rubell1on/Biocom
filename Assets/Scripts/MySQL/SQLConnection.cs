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

    public static async Task<MySqlConnection> GetConnection()
    {
            connectionString = GetConnectionString();
            connectionData = ConnectionData.Parse(connectionString);

        try
        {
            connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
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
        return PlayerPrefs.GetString("keyConnectionString");
    }
}