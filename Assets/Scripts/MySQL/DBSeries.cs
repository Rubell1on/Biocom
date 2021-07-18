using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class DBSeries
{
    public static List<Series> GetSeries()
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT * FROM {DBTableNames.series};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Series> serires = new List<Series>();

            while (reader.Read())
            {
                serires.Add(new Series(Convert.ToInt32(reader[0]),reader[1].ToString(), reader[2].ToString()));
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
    //public static bool AddUser(string userName, string password, string role)
    //{
    //    MySqlConnection connection = null;
    //    try
    //    {
    //        connection = SQLConnection.GetConnection();
    //        string sql = $"INSERT INTO {DBTableNames.users} SET username = \"{userName}\", password = \"{password}\", role = \"{role}\";";

    //        MySqlCommand command = new MySqlCommand(sql, connection);

    //        using (MySqlDataReader reader = command.ExecuteReader())
    //        {
    //            connection.Close();
    //            return true;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Ошибка: " + e);
    //        connection.Close();
    //        return false;
    //    }
    //}
    //public static bool RemoveUser(int id)
    //{
    //    MySqlConnection connection = null;
    //    try
    //    {
    //        connection = SQLConnection.GetConnection();
    //        string sql = $"DELETE FROM {DBTableNames.users} WHERE id = \"{id}\";";

    //        MySqlCommand command = new MySqlCommand(sql, connection);

    //        using (MySqlDataReader reader = command.ExecuteReader())
    //        {
    //            connection.Close();
    //            return true;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Ошибка: " + e);
    //        connection.Close();
    //        return false;
    //    }
    //}
    //public static bool EditUser(int id, string userName, string password, string role)
    //{
    //    MySqlConnection connection = null;

    //    try
    //    {
    //        connection = SQLConnection.GetConnection();
    //        string sql = $"UPDATE {DBTableNames.users} " +
    //            $"SET username = \"{userName}\", {(password.Length > 0 ? $"password = \"{password}\", " : "")} role = \"{role}\" " +
    //            $"WHERE id = \"{id}\";";

    //        MySqlCommand command = new MySqlCommand(sql, connection);

    //        using (MySqlDataReader reader = command.ExecuteReader())
    //        {
    //            connection.Close();
    //            return true;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Ошибка: " + e);
    //        connection.Close();
    //        return false;
    //    }
    //}
}
public class Series
{
    public int id;
    public string name;
    public string description;

    public Series(int id, string name, string description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}