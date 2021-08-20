using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPresetSelector : MonoBehaviour
{
    public ViewsController viewsController;
    public Dropdown presetSelector;

    void Start()
    {
        presetSelector.ClearOptions();
        List<string> presetNames = viewsController.presets.Select(p => p.name).ToList();
        presetSelector.AddOptions(presetNames);
        presetSelector.onValueChanged.AddListener(OnPresetChanged);

        viewsController.ApplyPreset(presetNames?[0]);
    }

    void OnPresetChanged(int id)
    {
        string name = viewsController.presets[id].name;
        viewsController.ApplyPreset(name);
    }
}
