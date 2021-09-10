using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MySQLConnection : MonoBehaviour
{
    public InputField ip;
    public InputField dbName;
    public InputField user;
    public InputField password;
    public Text textState;

    private string dirPath;
    private const string fileName = "connectionString.txt";

    private void Start()
    {
        dirPath = $"{Application.dataPath}/Resources";

        CheckRegistryAndFiles(dirPath, fileName);
        FillFieldUI();
    }

    private void CheckRegistryAndFiles(string dirPath, string fileName)
    {
        if (!PlayerPrefs.HasKey("keyIp") || !PlayerPrefs.HasKey("keyDBName") || !PlayerPrefs.HasKey("keyUser") || !PlayerPrefs.HasKey("keyPassword") || 
            !PlayerPrefs.HasKey("keyConnectionString"))
        {
            PlayerPrefs.SetString("keyIp", "192.168.0.1");
            PlayerPrefs.SetString("keyDBName", "biocom");
            PlayerPrefs.SetString("keyUser", "admin");
            PlayerPrefs.SetString("keyPassword", "1234567890");
            SetStringConnection(PlayerPrefs.GetString("keyIp"),
                PlayerPrefs.GetString("keyDBName"),
                PlayerPrefs.GetString("keyUser"),
                PlayerPrefs.GetString("keyPassword"));
        }

        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        if (!File.Exists($"{dirPath}/{fileName}"))
            SetStringConnection(PlayerPrefs.GetString("keyIp"), 
                PlayerPrefs.GetString("keyDBName"), 
                PlayerPrefs.GetString("keyUser"), 
                PlayerPrefs.GetString("keyPassword"));
    }

    private void FillFieldUI()
    {
        ip.text = PlayerPrefs.GetString("keyIp");
        dbName.text = PlayerPrefs.GetString("keyDBName");
        user.text = PlayerPrefs.GetString("keyUser");
        password.text = PlayerPrefs.GetString("keyPassword");
    }

    public void SaveParametrs()
    {
        PlayerPrefs.SetString("keyIp", ip.text);
        PlayerPrefs.SetString("keyDBName", dbName.text);
        PlayerPrefs.SetString("keyUser", user.text);
        PlayerPrefs.SetString("keyPassword", password.text);

        SetStringConnection(PlayerPrefs.GetString("keyIp"), 
            PlayerPrefs.GetString("keyDBName"), 
            PlayerPrefs.GetString("keyUser"), 
            PlayerPrefs.GetString("keyPassword"));
    }

    private void SetStringConnection(string ip, string dbName, string user, string password)
    {
        string connection = $"server={ip};database={dbName};user id={user};pwd={password};pooling=True;";
        PlayerPrefs.SetString("keyConnectionString", connection);
    }

    public async void CheckConnection()
    {
        string str = SQLConnection.GetConnectionString();
        if (await SQLConnection.GetConnection() == null)
        {
            textState.color = Color.red;
            textState.text = "Закрыто";
        }
        else
        {
            textState.color = Color.green;
            textState.text = "Открыто";
        }

    }
}
