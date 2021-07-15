using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

public static class DBUsers
{
    static string usersTable = "users";
    public static User Authorize(string user, string password)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT id, username, role FROM {usersTable} WHERE username = \"{user}\" AND password = \"{password}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            User userInstance = null;

            while (reader.Read())
            {
                userInstance = new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString());
            }
            reader.Close();

            if (userInstance != null)
                Debug.Log("Вход выполнен");
            else
                Debug.Log("Don't search users!!!!!");

            connection.Close();
            return userInstance;
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();
            return null;
        }
    }
    public static List<User> GetUsers()
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"SELECT id, username, role FROM {usersTable};";

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<User> users = new List<User>();

            while (reader.Read())
            {
                users.Add(new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString()));
            }
            reader.Close();

            connection.Close();
            return users;
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();

            return null;
        }
    }
    public static bool AddUser(string userName, string password, string role)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"INSERT INTO {usersTable} SET username = \"{userName}\", password = \"{password}\", role = \"{role}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
    public static bool RemoveUser(int id)
    {
        MySqlConnection connection = null;
        try
        {
            connection = SQLConnection.GetConnection();
            string sql = $"DELETE FROM {usersTable} WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
    public static bool EditUser(int id, string userName, string password, string role)
    {
        MySqlConnection connection = null;
        
        try
        { 
            connection = SQLConnection.GetConnection();
            string sql = $"UPDATE {usersTable} " +
                $"SET username = \"{userName}\", {(password.Length > 0 ? $"password = \"{password}\", " : "")} role = \"{role}\" " +
                $"WHERE id = \"{id}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                connection.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }
}
public class User
{
    public int id;
    public string userName;
    public enum Role { admin, user };
    public Role role;

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