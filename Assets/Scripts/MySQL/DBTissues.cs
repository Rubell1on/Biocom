using System;
using System.Threading.Tasks;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MySql.Data.MySqlClient;

class DBTissues
{
    public static async Task<List<Tissue>> GetTissues()
    {
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return await GetTissues(query);
    }

    public static async Task<List<Tissue>> GetTissues(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { "name", "rusName" };
            string query = queryBuilder.ToSearchQueryString(regexp);

            string sql = $"SELECT * FROM {DBTableNames.tissues}{(!String.IsNullOrEmpty(query) ? $" WHERE {query}" : "")};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Tissue> tissues = new List<Tissue>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                string name = reader[1].ToString();
                string rusName = reader[2].ToString();
                Color color;

                if (!ColorUtility.TryParseHtmlString(reader[3].ToString(), out color))
                {
                    Logger.GetInstance().Error("Can't parse color from string!");
                }

                tissues.Add(new Tissue(id, name, rusName, color));
            }
            reader.Close();

            connection.Close();
            return tissues;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }

    public static async Task<Tissue> GetTissueById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.tissues}.id", id.ToString() }
        };

        QueryBuilder query = new QueryBuilder(dictionary);
        List<Tissue> tissues = await GetTissues(query);
        if (tissues.Count > 0)
        {
            return tissues[0];
        }

        return null;
    }

    public static async Task<bool> RemoveTissue(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.tissues} WHERE id = \"{id}\";";

            Logger.GetInstance().Log("Отправлен запрос на удаление ткани");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Ткань успешно удалена.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static async Task<bool> AddTissue(string name, string rusName, Color32 color)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.tissues} SET name = \"{name}\", rusName = \"{rusName}\", color = \"#{ColorUtility.ToHtmlStringRGBA(color)}\";";

            Logger.GetInstance().Log("Отправлен запрос на добавление ткани");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Ткань {name}, добавлена в базу данных.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static async Task<bool> EditTissue(int id, string name, string rusName, Color32 color)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.tissues} SET name = \"{name}\", rusName = \"{rusName}\", color = \"#{ColorUtility.ToHtmlStringRGBA(color)}\" " +
                $"WHERE {DBTableNames.tissues}.id = {id};";

            Logger.GetInstance().Log("Отправлен запрос на редактирование ткани");

            bool result = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    
                    return true;
                }
            });

            Logger.GetInstance().Success($"Ткань изменена в базе данных.");
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

[Serializable]
public class Tissue
{
    public int id;
    public string name;
    public string rusName;
    public Color32 color = new Color32(255, 255, 255, 255);

    public Tissue()
    {
        this.id = -1;
        this.name = "default";
        this.rusName = "default";
    }

    public Tissue(int id, string name, string rusName, Color32 color)
    {
        this.id = id;
        this.name = name;
        this.rusName = rusName;
        this.color = color;
    }

    public Tissue(int id, string name, string rusName)
    {
        this.id = id;
        this.name = name;
        this.rusName = rusName;
    }
}