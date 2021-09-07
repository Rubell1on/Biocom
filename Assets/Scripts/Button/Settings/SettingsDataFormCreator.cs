using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class SettingsDataFormCreator : MonoBehaviour
{
    public GameObject settingsPanel;

    private GameObject panel;

    public void CreateOrDestroyForm()
    {
        if (panel == null)
            panel = Instantiate(settingsPanel, gameObject.transform);
        else
        {
            panel.GetComponent<ModalWindow>().Close();
            panel = null;
        }
    }

}
