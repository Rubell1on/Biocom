﻿using System.Threading.Tasks;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public class DBResearches 
{ 
    public static async Task<List<Research>> GetResearches()
    {
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return await GetResearches(query);
    }

    public static async Task<List<Research>> GetResearches(QueryBuilder queryBuilder, string groupBy = "")
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { "description", "note", "date" };
            string query = queryBuilder.ToSearchQueryString(regexp);
            string sql = $"SELECT {DBTableNames.researches}.id, {DBTableNames.researches}.date, {DBTableNames.researches}.description, {DBTableNames.researches}.note, {DBTableNames.researches}.state, {DBTableNames.users}.id, {DBTableNames.users}.username, {DBTableNames.researches}.sourceNiiFilePath " +
                $"FROM {SQLConnection.connectionData.database}.{DBTableNames.researches} " +
                $"JOIN {SQLConnection.connectionData.database}.{DBTableNames.users} ON {DBTableNames.researches}.userId = {DBTableNames.users}.id " +
                $"{(!String.IsNullOrEmpty(query) ? $"WHERE {query} " : "")}" +
                $"{(!String.IsNullOrEmpty(groupBy) ? $"GROUP BY {groupBy}" : "")};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Research> researches = new List<Research>();

            while (reader.Read())
            {
                researches.Add(new Research(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]), reader[6].ToString(), reader[7].ToString()));
            }
            reader.Close();

            connection.Close();

            return researches;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static async Task<Research> GetReasearchById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.researches}.id", id.ToString() }
        };

        QueryBuilder query = new QueryBuilder(dictionary);
        List<Research> researches = await GetResearches(query);
        if (researches.Count > 0)
        {
            return researches[0];
        }

        return null;
    }

    public static async Task<bool> AddResearch(int userId, string dateTime, string description, string note, string state)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.researches} SET userId = \"{userId}\", date = \"{dateTime}\", description = \"{description}\", note = \"{note}\", state = \"{state}\";";

            Logger.GetInstance().Log("Отправлен запрос на добавление исследования");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    
                    return true;
                }
            });

            Logger.GetInstance().Success($"Исследование добавлено в базу данных.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
    public static async Task<bool> RemoveResearch(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.researches} WHERE id = \"{id}\";";

            Logger.GetInstance().Log("Отправлен запрос на удаление исследования");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    
                    return true;
                }
            });

            Logger.GetInstance().Success($"Исследование удалено из базы данных.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static async Task<bool> EditResearch(int researchId, int userId, string description, string note, string state)
    {
        MySqlConnection connection = null;

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.researches} " +
                $"SET userId = \"{userId}\", description = \"{description}\", note = \"{note}\", state = \"{state}\" " +
                $"WHERE id = \"{researchId}\";";

            Logger.GetInstance().Log("Отправлен запрос на редактирование исследования");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Исследование успешно изменено.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
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
    public string seriesName;
    public enum State { newResearch, inProgress, finished };
    public State state;
    public string sourceNiiFilePath;

    public Research(int id, string date, string description, string note, string state, int userId, string userName, string sourceNiiFilePath = "")
    {
        this.id = id;
        this.date = DateTime.Parse(date);
        this.description = description;
        this.note = note;
        this.state = GetState(state);
        this.userId = userId;
        this.userName = userName;
        this.sourceNiiFilePath = sourceNiiFilePath;
    }

    private static State GetState(string state)
    {
        return (State)Enum.GetNames(typeof(State)).ToList().FindIndex(s => s == state);
    }
}