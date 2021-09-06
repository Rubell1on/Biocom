using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MySql.Data.MySqlClient;

class DBTissues
{
    public static List<Tissue> GetTissues()
    {
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return GetTissues(query);
    }

    public static List<Tissue> GetTissues(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
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

    public static Tissue GetTissueById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.tissues}.id", id.ToString() }
        };

        QueryBuilder query = new QueryBuilder(dictionary);
        List<Tissue> tissues = GetTissues(query);
        if (tissues.Count > 0)
        {
            return tissues[0];
        }

        return null;
    }

    public static bool RemoveTissue(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.tissues} WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Ткань успешно удалена.");
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

    public static bool AddTissue(string name, string rusName, Color32 color)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.tissues} SET name = \"{name}\", rusName = \"{rusName}\", color = \"#{ColorUtility.ToHtmlStringRGBA(color)}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Ткань {name}, добавлена в базу данных.");
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

    public static bool EditTissue(int id, string name, string rusName, Color32 color)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.tissues} SET name = \"{name}\", rusName = \"{rusName}\", color = \"#{ColorUtility.ToHtmlStringRGBA(color)}\" " +
                $"WHERE {DBTableNames.tissues}.id = {id};";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Ткань изменена в базе данных.");
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