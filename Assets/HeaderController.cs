using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatchyClick;

public class HeaderController : MonoBehaviour
{
    public List<GameObject> uiObject;
    public DataGridViewRowSelector rowSelector;
    
    private void OnEnable()
    {
        rowSelector.rowSelectionChanged.AddListener(OnRowSelectionChanged);
        if (rowSelector.selectedRow == null)
            Disable();
    }
    private void OnDisable()
    {
        rowSelector.rowSelectionChanged.RemoveListener(OnRowSelectionChanged);
    }

    public void OnRowSelectionChanged(DataGridViewRowUI row)
    {
        if (row != null)
            Enable();
        else
            Disable();
    }

    public void Disable()
    {
        uiObject.ForEach(c => c.GetComponent<Button>().interactable = false);
    }
    public void Enable()
    {
        uiObject.ForEach(c => c.GetComponent<Button>().interactable = true);
    }
}
