using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomForm : MonoBehaviour
{
    public List<CustomInputField> inputs = new List<CustomInputField>();
    public CustomButton submitButton;
    public UnityEvent Submit = new UnityEvent();

    [HideInInspector]
    public UnityEvent Validate = new UnityEvent();
    public ValidationFailedEvent ValidationFailed = new ValidationFailedEvent();

    void Start()
    {
        Init();
        Validate.AddListener(CheckFields);
    }

    private void CheckFields()
    {
        bool result = inputs.Where(i => i.required == true).All(i => !String.IsNullOrEmpty(i.text));
        if (result)
        {
            Submit.Invoke();
        }
        else
        {
            ValidationFailed.Invoke("Ќе все об€зательные пол€ были заполнены");
        }
    }

    private void Init()
    {
        inputs.ForEach(i => i.form = this);
        if (submitButton != null) submitButton.form = this;
    }

    public class ValidationFailedEvent : UnityEvent<string> { }
}
