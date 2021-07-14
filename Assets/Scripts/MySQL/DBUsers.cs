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
        MySqlConnection connection = SQLConnection.GetConnection();
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

        return userInstance;
    }
    public static List<User> GetUsers()
    {
        MySqlConnection connection = SQLConnection.GetConnection();
        string sql = $"SELECT id, username, role FROM {usersTable};";

        MySqlCommand command = new MySqlCommand(sql, connection);
        MySqlDataReader reader = command.ExecuteReader();
        List<User> users = new List<User>();

        while (reader.Read())
        {
            users.Add(new User(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString()));
        }
        reader.Close();

        return users;
    }
    public static bool AddUser(string userName, string password, string role)
    {
        try
        {
            MySqlConnection connection = SQLConnection.GetConnection();
            string sql = $"INSERT INTO {usersTable} SET username = \"{userName}\", password = \"{password}\", role = \"{role}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
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