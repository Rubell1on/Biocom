using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePassword : MonoBehaviour
{
    public InputField inputField;

    public void ChangeContentType()
    {
        inputField.contentType = inputField.contentType == InputField.ContentType.Password 
            ? InputField.ContentType.Standard 
            : InputField.ContentType.Password;
    }
}
