using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestViewController : MonoBehaviour
{
    public ViewsController viewsController;
    public InputField row;
    public InputField col;
    public Button submit;

    // Start is called before the first frame update
    void Start()
    {
        submit.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        int i = Convert.ToInt32(row.text);
        int j = Convert.ToInt32(col.text);

        viewsController.AddView(new Vector2(i, j));
    }
}
