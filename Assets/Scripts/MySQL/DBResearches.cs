using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public class DBResearches 
{ 
    public static List<Research> GetResearches()
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT {DBTableNames.researches}.id, {DBTableNames.researches}.date, {DBTableNames.researches}.description, {DBTableNames.researches}.note, {DBTableNames.researches}.state, {DBTableNames.users}.id, {DBTableNames.users}.username, {DBTableNames.researches}.seriesId " +
                $"FROM {SQLConnection.database}.{DBTableNames.researches} " +
                $"JOIN {SQLConnection.database}.{DBTableNames.users} " +
                $"ON {DBTableNames.researches}.userId = {DBTableNames.users}.id;";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Research> researches = new List<Research>();

            while (reader.Read())
            {
                researches.Add(new Research(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]), reader[6].ToString(), Convert.ToInt32(reader[7].ToString())));
            }
            reader.Close();

            connection.Close();
            return researches;
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static List<Research> GetResearchesByUserId(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT {DBTableNames.researches}.id, {DBTableNames.researches}.date, {DBTableNames.researches}.description, {DBTableNames.researches}.note, {DBTableNames.researches}.state, {DBTableNames.users}.id, {DBTableNames.users}.username, {DBTableNames.researches}.seriesId " +
                $"FROM {SQLConnection.database}.{DBTableNames.researches} " +
                $"JOIN {SQLConnection.database}.{DBTableNames.users} " +
                $"ON {DBTableNames.researches}.userId = {DBTableNames.users}.id " +
                $"WHERE {DBTableNames.users}.id = {id}";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Research> researches = new List<Research>();

            while (reader.Read())
            {
                researches.Add(new Research(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]), reader[6].ToString(), Convert.ToInt32(reader[7].ToString())));
            }
            reader.Close();

            connection.Close();
            return researches;
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
    //        string sql = $"INSERT INTO {tableName} SET username = \"{userName}\", password = \"{password}\", role = \"{role}\";";

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
    //        string sql = $"DELETE FROM {tableName} WHERE id = \"{id}\";";

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
    //        string sql = $"UPDATE {tableName} " +
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
public class Research
{
    public int id;
    public DateTime date;
    public string description;
    public string note;
    public int userId;
    public string userName;
    public int seriesId;
    public enum State { newResearch, inProgress, finished };
    public State state;

    public Research(int id, string date, string description, string note, string state, int userId, string userName, int seriesId)
    {
        this.id = id;
        this.date = DateTime.Parse(date);
        this.description = description;
        this.note = note;
        this.state = GetState(state);
        this.userId = userId;
        this.userName = userName;
        this.seriesId = seriesId;
    }

    private static State GetState(string state)
    {
        return (State)Enum.GetNames(typeof(State)).ToList().FindIndex(s => s == state);
    }
}