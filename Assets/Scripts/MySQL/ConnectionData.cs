﻿using System;
using System.Collections.Generic;

public class ConnectionData
{
    public string server;
    public string database;
    public string user;
    public string password;
    public bool pooling;

    public ConnectionData() { }

    public ConnectionData(string server, string database, string user, string password, bool pooling = true)
    {
        this.server = server;
        this.database = database;
        this.user = user;
        this.password = password;
        this.pooling = pooling;
    }

    public static ConnectionData Parse(string connectionString)
    {
        ConnectionData data = new ConnectionData();
        Dictionary<string, Action<string>> actions = new Dictionary<string, Action<string>>() {
            { "server", (string s) => data.server = s },
            { "database", (string s) => data.database = s },
            { "user id", (string s) => data.user = s},
            { "pwd", (string s) => data.password = s },
            { "pooling", (string s) => data.pooling = s == "True" ? true : false }
        };

        string[] parameters = connectionString.Trim().Split(';');

        foreach (string parameter in parameters)
        {
            string[] field = parameter.Split('=');
            if (field.Length == 2)
            {
                string key = field[0].ToLower();
                string value = field[1];
                actions[key].Invoke(value);
            }
            else
            {
                continue;
            }
        }

        return data;
    }

    public override string ToString()
    {
        List<string> parameters = new List<string>()
        {
            !String.IsNullOrEmpty(this.server) ? $"server={this.server};" : "",
            !String.IsNullOrEmpty(this.database) ? $"database={this.database};" : "",
            !String.IsNullOrEmpty(this.user) ? $"user id={this.user};" : "",
            !String.IsNullOrEmpty(this.password) ? $"pwd={this.password};" : "",
            $"pooling={(this.pooling ? "True" : "False")};"
        };

        return String.Join("", parameters);
    }
}