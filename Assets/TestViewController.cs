using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestViewController : MonoBehaviour
{
    public ViewsController viewsController;
    public InputField row;
    public InputField col;
    public Button add;
    public Button remove;
    public Button removeAll;
    public Dropdown presetSelector;

    // Start is called before the first frame update
    void Start()
    {
        add.onClick.AddListener(Add);
        remove.onClick.AddListener(Remove);
        removeAll.onClick.AddListener(viewsController.RemoveAllViews);

        presetSelector.ClearOptions();
        List<string> presetNames = viewsController.presets.Select(p => p.name).ToList();
        presetSelector.AddOptions(presetNames);
        presetSelector.onValueChanged.AddListener(OnPresetChanged);
    }

    void OnPresetChanged(int id)
    {
        string name = viewsController.presets[id].name;
        viewsController.ApplyPreset(name);
    }

    void Add()
    {
        int i = Convert.ToInt32(row.text);
        int j = Convert.ToInt32(col.text);

        viewsController.AddView(new Vector2(i, j));
    }

    void Remove()
    {
        int i = Convert.ToInt32(row.text);
        int j = Convert.ToInt32(col.text);

        viewsController.RemoveView(new Vector2(i, j));
    }
}
