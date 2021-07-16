using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
public class DBParts : MonoBehaviour
{
    public static List<Part> GetParts()
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT * FROM {DBTableNames.parts};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Part> serires = new List<Part>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                int resId = Convert.ToInt32(reader[1]);
                int remoteId = Convert.ToInt32(reader[2]);
                string filePath = reader[3].ToString();
                string partName = reader[4].ToString();

                serires.Add(new Part(id, partName, resId, remoteId, filePath));
            }
            reader.Close();

            connection.Close();
            return serires;
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static List<Part> GetPartsByResearchId(int researchId)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT * FROM {DBTableNames.parts} WHERE {DBTableNames.parts}.researchId = \"{researchId}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Part> serires = new List<Part>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                int resId = Convert.ToInt32(reader[1]);
                int remoteId = Convert.ToInt32(reader[2]);
                string filePath = reader[3].ToString();
                string partName = reader[4].ToString();

                serires.Add(new Part(id, partName, resId, remoteId, filePath));
            }
            reader.Close();

            connection.Close();
            return serires;
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }
}

public class Part
{
    public int id;
    public int researchId;
    public int remoteId;
    public string filePath;
    public string partName;

    public Part(int id, string partName, int researchId, int remoteId, string filePath)
    {
        this.id = id;
        this.researchId = researchId;
        this.remoteId = remoteId;
        this.filePath = filePath;
        this.partName = partName;
    }
}