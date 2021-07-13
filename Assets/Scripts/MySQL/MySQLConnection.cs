using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

public class MySQLConnection : MonoBehaviour
{
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection connection = null;
    private MySqlCommand command = null;
    private MySqlDataReader reader = null;

    void Awake()
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
            Debug.Log("Mysql state: " + connection.State);

            string sql = "SELECT * FROM deviceinfo";
            command = new MySqlCommand(sql, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Debug.Log("???");
                Debug.Log(reader[0] + " -- " + reader[1]);
            }
            reader.Close();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
