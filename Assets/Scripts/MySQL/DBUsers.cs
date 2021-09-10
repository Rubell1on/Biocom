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
            
            Logger.GetInstance().Log("Запрос на авторизацию отправлен");

            User userInstance = await Task.Run(() =>
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();

                User userInstance = null;

                while (reader.Read())
                {
                    userInstance = new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString());
                }
                reader.Close();

                return userInstance;
            });

            if (userInstance != null)
                Logger.GetInstance().Success($"Здравствуйте, {user}.");
            else
                Logger.GetInstance().Warning($"Внимание. Введен не верный логин/пароль. Попробуйте, еще раз.");

            connection.Close();
            return userInstance;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
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

            Logger.GetInstance().Log("Запрос на добавление пользователя отправлен");
            bool result = await Task.Run(() =>
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Пользователь {userName}, добавлен в базу данных.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
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

            Logger.GetInstance().Log("Запрос на удаление пользователя отправлен");

            bool result = await Task.Run(() =>
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Logger.GetInstance().Success($"Пользователь, удален из базы данных.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
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

            Logger.GetInstance().Log("Запрос на редактирование пользователя отправлен");

            bool result = await Task.Run(() =>
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    
                    return true;
                }
            });

            Logger.GetInstance().Success($"Пользователь изменен.");
            return result;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();
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

            List<User> users = await Task.Run(() =>
            {
                MySqlDataReader reader = command.ExecuteReader();
                List<User> users = new List<User>();

                while (reader.Read())
                {
                    users.Add(new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString()));
                }

                reader.Close();

                connection.Close();

                return users;
            });

            return users;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

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
            
            User user = await Task.Run(() =>
            {
                MySqlDataReader reader = command.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    user = new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString());
                }

                reader.Close();
                connection.Close();

                return user;
            });

            return user;
        }
        catch (Exception e)
        {
            Logger.GetInstance().Error("Ошибка: " + e);
            connection.Close();

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