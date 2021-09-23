using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public CustomForm form;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FormCheck(form);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        FormCheck(form);
    }

    private void FormCheck(CustomForm form)
    {
        if (form != null)
        {
            form.Validate.Invoke();
        }
    }
}
