using System;
using System.Collections.Generic;

public class ConnectionData
{
    public string server;
    public string database;
    public string user;
    public string password;
    public bool pooling;

    public ConnectionData() { }

    public ConnectionData(string server = "192.168.0.1", string database = "biocom", string user = "admin", string password = "1234567890", bool pooling = true)
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
                string key = field[0].ToLower().Trim();
                string value = field[1].Trim();
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
            $"server={this.server};",
            $"database={this.database};",
            $"user id={this.user};",
            $"pwd={this.password};",
            $"pooling=True"
        };

        return String.Join("", parameters);
    }

    public override bool Equals(object obj)
    {
        ConnectionData newData = (ConnectionData)obj;
        if (this.server == newData.server && this.database == newData.database && this.user == newData.user && this.password == newData.password && this.pooling && newData.pooling)
        {
            return true;
        }

        return false;
    }
}
