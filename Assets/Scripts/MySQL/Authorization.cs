using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    public Button enterButton;
    public Button exitButton;
    public List<Text> userNames;
    public InputField login;
    public InputField password;
    public CanvasController canvasController;
    public User userData;

    private void Start()
    {
        enterButton.onClick.AddListener(Authorize);
        exitButton.onClick.AddListener(Exit);
    }

    private void Authorize()
    {
        userData = DBUsers.Authorize(login.text, password.text);
        if (userData != null)
        {
            userNames.ForEach(text =>
            {
                text.text = login.text;
            });
            canvasController.SelectCanvas((int)userData.role + 1);
        }
    }

    private void Exit()
    {
        Application.Quit();
    }
}
