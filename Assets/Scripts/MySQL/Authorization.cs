using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    public Button button;
    public List<Text> userNames;
    public InputField login;
    public InputField password;
    public CanvasController canvasController;
    public User userData;

    private void Start()
    {
        button.onClick.AddListener(Authorize);
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
}
