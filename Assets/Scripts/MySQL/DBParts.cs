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
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return GetParts(query);
    }

    public static List<Part> GetParts(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { $"{DBTableNames.parts}.filePath", $"{DBTableNames.series}.name" };
            string query = queryBuilder.ToQueryString(regexp);
            string sql = $"SELECT {DBTableNames.parts}.id, {DBTableNames.parts}.seriesId, {DBTableNames.parts}.remoteId, {DBTableNames.parts}.filePath, {DBTableNames.parts}.tissueId, {DBTableNames.series}.name, {DBTableNames.tissues}.name, {DBTableNames.tissues}.rusName, {DBTableNames.tissues}.color " +
                $"FROM {DBTableNames.parts} " +
                $"JOIN {DBTableNames.series} ON {DBTableNames.parts}.seriesId = {DBTableNames.series}.id " +
                $"LEFT JOIN {DBTableNames.tissues} ON {DBTableNames.parts}.tissueId = {DBTableNames.tissues}.id " +
                $"{(!String.IsNullOrEmpty(query) ? $" WHERE {query}" : "")};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Part> serires = new List<Part>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                int serId = Convert.ToInt32(reader[1]);
                int remoteId = Convert.ToInt32(reader[2]);
                string filePath = reader[3].ToString();
                string seriesName = reader[5].ToString();

                int tissueId = Convert.ToInt32(reader[4]);
                string tissueName = reader[6].ToString();
                string tissueRusName = reader[7].ToString();

                Color color = new Color();

                ColorUtility.TryParseHtmlString(reader[8].ToString(), out color);

                Tissue tissue = new Tissue(tissueId, tissueName, tissueRusName, color);
                Part part = new Part(id, serId, remoteId, filePath, seriesName);
                part.tissue = tissueId != -1 ? tissue : new Tissue(tissueId, "default", "default");

                serires.Add(part);
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

    public static Part GetPart(int partId)
    {
        MySqlConnection connection = null;
        Part part = null;

        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT {DBTableNames.parts}.id, {DBTableNames.parts}.seriesId, {DBTableNames.parts}.remoteId, {DBTableNames.parts}.filePath, {DBTableNames.parts}.tissueId, {DBTableNames.series}.name, {DBTableNames.tissues}.name, {DBTableNames.tissues}.rusName, {DBTableNames.tissues}.color " +
                $"FROM {DBTableNames.parts} " +
                $"JOIN {DBTableNames.series} ON {DBTableNames.parts}.seriesId = {DBTableNames.series}.id " +
                $"LEFT JOIN {DBTableNames.tissues} ON {DBTableNames.parts}.tissueId = {DBTableNames.tissues}.id " +
                $"WHERE {DBTableNames.parts}.id = \"{partId}\"";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                int serId = Convert.ToInt32(reader[1]);
                int remoteId = Convert.ToInt32(reader[2]);
                string filePath = reader[3].ToString();
                string seriesName = reader[5].ToString();

                int tissueId = Convert.ToInt32(reader[4]);

                string tissueName = reader[6].ToString();
                string tissueRusName = reader[7].ToString();

                Color color = new Color();

                ColorUtility.TryParseHtmlString(reader[8].ToString(), out color);
                Tissue tissue = new Tissue(tissueId, tissueName, tissueRusName, color);
                part = new Part(id, serId, remoteId, filePath, seriesName, tissue);
            }
            reader.Close();

            connection.Close();
            return part;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
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
            string sql = $"SELECT {DBTableNames.parts}.id, {DBTableNames.parts}.seriesId, {DBTableNames.parts}.remoteId, {DBTableNames.parts}.filePath, {DBTableNames.parts}.tissueId, {DBTableNames.series}.name, {DBTableNames.tissues}.name, {DBTableNames.tissues}.rusName, {DBTableNames.tissues}.color " +
                $"FROM {DBTableNames.parts} " +
                $"JOIN {DBTableNames.series} ON {DBTableNames.parts}.seriesId = {DBTableNames.series}.id " +
                $"JOIN {DBTableNames.researches} ON {DBTableNames.series}.researchId = {DBTableNames.researches}.id " +
                $"LEFT JOIN {DBTableNames.tissues} ON {DBTableNames.parts}.tissueId = {DBTableNames.tissues}.id " +
                $"WHERE {DBTableNames.researches}.id = \"{researchId}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Part> serires = new List<Part>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                int serId = Convert.ToInt32(reader[1]);
                int remoteId = Convert.ToInt32(reader[2]);
                string filePath = reader[3].ToString();
                string seriesName = reader[5].ToString();

                int tissueId = Convert.ToInt32(reader[4]);
                string tissueName = reader[6].ToString();
                string tissueRusName = reader[7].ToString();

                Color color = new Color();

                ColorUtility.TryParseHtmlString(reader[8].ToString(), out color);
                Tissue tissue = new Tissue(tissueId, tissueName, tissueRusName, color);

                serires.Add(new Part(id, serId, remoteId, filePath, seriesName, tissue));
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

    public static bool AddPart(int seriesId, int tissueId, string filePath, int remoteId = -1)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.parts} " +
                $"SET {DBTableNames.parts}.seriesId = \"{seriesId}\", {DBTableNames.parts}.tissueId = \"{tissueId}\", {DBTableNames.parts}.filePath = \"{filePath}\", {DBTableNames.parts}.remoteId = \"{remoteId}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Элемент, добавлен в базу данных.");
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

    public static bool EditPart(int id, int seriesId, int tissieId, string filePath, int remoteId = -1)
    {
        MySqlConnection connection = null;

        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.parts} " +
                $"SET seriesId = \"{seriesId}\", tissueId = \"{tissieId}\", filePath = \"{filePath}\", remoteId = \"{remoteId}\" " +
                $"WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Элемент успешно изменен.");
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

    public static bool RemovePart(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.parts} WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                Logger.GetInstance().Log($"Элемент удален из базы данных.");
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

public class Part
{
    public int id;
    public int seriesId;
    public int remoteId;
    public string filePath;
    public Tissue tissue;
    public string seriesName;

    public Part(int id, int seriesId, int remoteId, string filePath, string seriesName)
    {
        this.id = id;
        this.seriesId = seriesId;
        this.remoteId = remoteId;
        this.filePath = filePath;
        this.seriesName = seriesName;
        this.tissue = new Tissue();
    }

    public Part(int id, int seriesId, int remoteId, string filePath, string seriesName, Tissue tissue)
    {
        this.id = id;
        this.seriesId = seriesId;
        this.remoteId = remoteId;
        this.filePath = filePath;
        this.seriesName = seriesName;
        this.tissue = tissue;
    }

    public static Dictionary<int, List<Part>> GetSeries(List<Part> parts)
    {
        List<int> keysSet = new HashSet<int>(parts.Select(p => p.seriesId)).ToList();
        Dictionary<int, List<Part>> series = new Dictionary<int, List<Part>>();
        keysSet.ForEach(k => series[k] = new List<Part>());
        parts.ForEach(p => series[p.seriesId].Add(p));

        return series;
    }
}