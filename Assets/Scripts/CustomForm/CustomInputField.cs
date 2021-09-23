using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputField : InputField
{
    public CustomForm form;
    public bool required = false;

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        if (form != null)
        {
            form.Validate.Invoke();
        }
    }
}