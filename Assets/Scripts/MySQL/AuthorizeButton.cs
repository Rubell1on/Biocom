using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizeButton : MonoBehaviour
{
    public Button button;
    public InputField login;
    public InputField password;
    public CanvasController canvasController;

    private void Start()
    {
        button.onClick.AddListener(Authorize);
    }

    private void Authorize()
    {
        User user = DBUsers.Authorize(login.text, password.text);
        if (user != null)
        {
            canvasController.SelectCanvas((int)user.role + 1);
        }
    }
}
