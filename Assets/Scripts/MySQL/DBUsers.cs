using System.Threading.Tasks;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class DBUsers
{
    public static async Task<User> Authorize(string user, string password)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"SELECT id, username, role FROM {DBTableNames.users} WHERE username = \"{user}\" AND password = \"{password}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);
            DbDataReader reader = await command.ExecuteReaderAsync();
            User userInstance = null;

            while (await reader.ReadAsync())
            {
                userInstance = new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString());
            }
            reader.Close();

            if (userInstance != null)
                Logger.GetInstance().Log($"Здравствуйте, {user}.");
            else
                Logger.GetInstance().Warning($"Внимание. Введен не верный логин/пароль. Попробуйте, еще раз.");

            await connection.CloseAsync();
            return userInstance;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();
            return null;
        }
    }
    public static async Task<List<User>> GetUsers()
    {
        QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
        return await GetUsers(query);
    }

    public static async Task<bool> AddUser(string userName, string password, string role)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.users} SET username = \"{userName}\", password = \"{password}\", role = \"{role}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                await connection.CloseAsync();
                Logger.GetInstance().Log($"Пользователь {userName}, добавлен в базу данных.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();
            return false;
        }
    }

    public static async Task<bool> RemoveUser(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"DELETE FROM {DBTableNames.users} WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                await connection.CloseAsync();
                Logger.GetInstance().Log($"Пользователь, удален из базы данных.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();
            return false;
        }
    }

    public static async Task<bool> EditUser(int id, string userName, string password, string role)
    {
        MySqlConnection connection = null;
        
        try
        { 
            connection = await SQLConnection.GetConnection();
            string sql = $"UPDATE {DBTableNames.users} " +
                $"SET username = \"{userName}\", {(password.Length > 0 ? $"password = \"{password}\", " : "")} role = \"{role}\" " +
                $"WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                await connection.CloseAsync();
                Logger.GetInstance().Log($"Пользователь изменен.");
                return true;
            }
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();
            return false;
        }
    }

    public static async Task<User> GetUserById(int id)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.users}.id", id.ToString() }
        };

        QueryBuilder query = new QueryBuilder(dictionary);
        List<User> users = await GetUsers(query);
        if (users.Count > 0)
        {
            return users[0];
        }

        return null;
    }

    public static async Task<List<User>> GetUsers(QueryBuilder queryBuilder)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            List<string> regexp = new List<string>() { "userName" };
            string query = queryBuilder.ToSearchQueryString(regexp);
            string sql = $"SELECT id, username, role FROM {DBTableNames.users}{(!String.IsNullOrEmpty(query) ? $" WHERE {query}" : "")};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            DbDataReader reader = await command.ExecuteReaderAsync();
            List<User> users = new List<User>();

            while (await reader.ReadAsync())
            {
                users.Add(new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString()));
            }

            reader.Close();

            await connection.CloseAsync();
            return users;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();

            return null;
        }
    }

    public static async Task<User> GetUserByResearchId(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"SELECT {DBTableNames.users}.id, {DBTableNames.users}.username, {DBTableNames.users}.role " +
                $"FROM {SQLConnection.connectionData.database}.{DBTableNames.researches} " +
                $"JOIN {SQLConnection.connectionData.database}.{DBTableNames.users} " +
                $"ON {SQLConnection.connectionData.database}.{DBTableNames.researches}.userId = {SQLConnection.connectionData.database}.{DBTableNames.users}.id " +
                $"WHERE {SQLConnection.connectionData.database}.{DBTableNames.researches}.id = \"{id}\";";
                //$"GROUP BY {SQLConnection.connectionData.database}.{DBTableNames.users}.id";

            MySqlCommand command = new MySqlCommand(sql, connection);
            DbDataReader reader = await command.ExecuteReaderAsync();

            User user = null;
            if (await reader.ReadAsync())
            {
                user = new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString());
            }

            reader.Close();
            await connection.CloseAsync();

            return user;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            await connection.CloseAsync();

            return null;
        }
    }
}

[Serializable]
public class User
{
    public int id;
    public string userName;
    public string password;
    public enum Role { admin, user };
    public Role role;

    public User() { }

    public User(int id, string userName, string role)
    {
        this.id = id;
        this.userName = userName;
        this.role = GetRole(role);
    }

    private static Role GetRole(string role)
    {
        return (Role)Enum.GetNames(typeof(Role)).ToList().FindIndex(s => s == role);
    }
}