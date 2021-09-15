using System.Threading.Tasks;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class DBSeries
{
    public static async Task<List<Series>> GetSeries()
    {
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return await GetSeries(query);
    }

    public static async Task<List<Series>> GetSeries(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { "name", $"{DBTableNames.series}.description" };
            string query = queryBuilder.ToSearchQueryString(regexp);
            string sql = $"SELECT {DBTableNames.series}.id, {DBTableNames.series}.name, {DBTableNames.series}.description, {DBTableNames.researches}.id,  {DBTableNames.series}.sourceNiiFilePath " +
                $"FROM {DBTableNames.series} " +
                $"LEFT JOIN {DBTableNames.researches} ON {DBTableNames.researches}.id = {DBTableNames.series}.researchId" +
                $"{(!String.IsNullOrEmpty(query) ? $" WHERE {query}" : "")};";

            List<Series> series = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<Series> series = new List<Series>();

                while (reader.Read())
                {
                    int researchId;
                    if (!Int32.TryParse(reader[3].ToString(), out researchId))
                    {
                        researchId = -1;
                    }

                    series.Add(new Series(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), researchId, reader[4].ToString()));
                }
                reader.Close();

                connection.Close();

                return series;
            });

            return series;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static async Task<Series> GetSeriesById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.series}.id", id.ToString() }
        };

        List<Series> series = await GetSeries(new QueryBuilder(dictionary));
        if (series.Count > 0)
        {
            return series[0];
        }

        return null;
    }

    public static async Task<List<Series>> GetSeriesByResearchId(int researchId)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.series}.researchId", researchId.ToString() }
        };

        List<Series> series = await GetSeries(new QueryBuilder(dictionary));
        if (series.Count > 0)
        {
            return series;
        }

        return null;
    }

    public static async Task<bool> AddSeries(string seriesName, string description, int researchId)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.series} SET name = \"{seriesName}\", description = \"{description}\", researchId = \"{researchId}\";";

            Logger.GetInstance().Log("Отправлен запрос на добавление серии");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Серия {seriesName}, добавлена в базу данных.");
            return result;
            
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
    public static async Task<bool> RemoveSeries(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.series} WHERE id = \"{id}\";";

            Logger.GetInstance().Log("Отправлен запрос на удаление серии");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Серия успешно удалена.");

            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static async Task<bool> EditSeries(int id, string seriesName, string description, int researchId)
    {
        MySqlConnection connection = null;

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.series} " +
                $"SET name = \"{seriesName}\", description = \"{description}\", researchId = \"{researchId}\" " +
                $"WHERE id = \"{id}\";";

            Logger.GetInstance().Log("Отправлен запрос на редактирование серии");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Серия изменина в базе данных.");
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
public class Series
{
    public int id;
    public string name;
    public string description;
    public int researchId;
    public string sourceNiiFilePath;
    public List<Part> parts = new List<Part>();
    public Texture2D thumbnail = null;
    public int photosCount = 0;

    public Series(int id, string name, string description, int researchId, string sourceNiiFilePath = "")
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.researchId = researchId;
        this.sourceNiiFilePath = sourceNiiFilePath;
    }
}