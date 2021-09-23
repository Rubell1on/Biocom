using System.Threading.Tasks;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    public Button exitButton;
    public List<Text> userNames;
    public InputField loginField;
    public InputField passwordField;
    public Toggle savePassword;
    public CanvasController canvasController;
    public User userData;
    public CustomForm authorizationForm;

    private static readonly string login = "login";
    private static readonly string password = "password";

    private void Awake()
    {
        MySQLConnection mc = new MySQLConnection();
        mc.CheckRegistry();
    }
    private void Start()
    {
        authorizationForm.ValidationFailed.AddListener(OnValidationFailed);
        authorizationForm.Submit.AddListener(Authorize);
        exitButton.onClick.AddListener(Exit);
        
        if (PlayerPrefs.HasKey(login) && PlayerPrefs.HasKey(password))
        {
            User user = GetUserData();
            loginField.text = user.userName;
            passwordField.text = user.password;
            savePassword.isOn = true;

            Authorize();
        }
        else
        {
            savePassword.isOn = false;
        }
    }

    public async void Authorize()
    {
        userData = await DBUsers.Authorize(loginField.text, passwordField.text);
        if (userData != null)
        {
            if (savePassword.isOn)
            {
                SetUserData(new User() { userName = loginField.text, password = passwordField.text });
            }
            else
            {
                ClearUserData();
            }

            userNames.ForEach(text =>
            {
                text.text = loginField.text;
            });
            canvasController.SelectCanvas((int)userData.role + 1);
        }

        return;
    }

    private User GetUserData()
    {
        if (PlayerPrefs.HasKey(login) && PlayerPrefs.HasKey(password)) 
        {
            string userName = PlayerPrefs.GetString(login);
            string pwd = PlayerPrefs.GetString(password);

            return new User() { userName = userName, password = pwd };
        }

        return null;
    }

    private void SetUserData(User user)
    {
        PlayerPrefs.SetString(login, user.userName);
        PlayerPrefs.SetString(password, user.password);
    }

    private void ClearUserData()
    {
        if (PlayerPrefs.HasKey(login)) PlayerPrefs.DeleteKey(login);
        if (PlayerPrefs.HasKey(password)) PlayerPrefs.DeleteKey(password);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void OnValidationFailed(string message)
    {
        Logger.GetInstance().Error(message);
    }
}
