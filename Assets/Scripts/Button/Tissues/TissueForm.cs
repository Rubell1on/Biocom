using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class TissueForm : MonoBehaviour
{
    public InputField tissueName;
    public InputField tissueRusName;
    public InputField color;

    public Text label;
    public Button apply;

    public void SetInfo(string buttonText, string labelText)
    {
        apply.GetComponentInChildren<Text>().text = buttonText;
        label.text = labelText;
    }
}