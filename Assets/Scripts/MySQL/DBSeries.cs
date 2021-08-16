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
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return GetSeries(query);
    }

    public static List<Series> GetSeries(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { "name", $"{DBTableNames.series}.description" };
            string query = queryBuilder.ToQueryString(regexp);
            string sql = $"SELECT {DBTableNames.series}.id, {DBTableNames.series}.name, {DBTableNames.series}.description, {DBTableNames.researches}.id " +
                $"FROM {DBTableNames.series} " +
                $"LEFT JOIN {DBTableNames.researches} ON {DBTableNames.researches}.id = {DBTableNames.series}.researchId" +
                $"{(!String.IsNullOrEmpty(query) ? $" WHERE {query}" : "")};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Series> serires = new List<Series>();

            while (reader.Read())
            {
                int researchId;
                if (!Int32.TryParse(reader[3].ToString(), out researchId))
                {
                    researchId = -1;
                }

                serires.Add(new Series(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), researchId));
            }
            reader.Close();

            connection.Close();
            return serires;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static Series GetSeriesById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.series}.id", id.ToString() }
        };

        List<Series> series = GetSeries(new QueryBuilder(dictionary));
        if (series.Count > 0)
        {
            return series[0];
        }

        return null;
    }

    public static bool AddSeries(string seriesName, string description, int researchId)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.series} SET name = \"{seriesName}\", description = \"{description}\", researchId = \"{researchId}\";";
            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Серия {seriesName}, добавлена в базу данных.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
    public static bool RemoveSeries(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.series} WHERE id = \"{id}\";";
            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Серия успешно удалена.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static bool EditSeries(int id, string seriesName, string description, int researchId)
    {
        MySqlConnection connection = null;

        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.series} " +
                $"SET name = \"{seriesName}\", description = \"{description}\", researchId = \"{researchId}\" " +
                $"WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Серия изменина в базе данных.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
}
public class Series
{
    public int id;
    public string name;
    public string description;
    public int researchId;

    public Series(int id, string name, string description, int researchId)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.researchId = researchId;
    }
}