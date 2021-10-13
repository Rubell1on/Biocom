using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class KnowledgeBaseDataForm : MonoBehaviour
{
    public Button applyButton;
    public Text text;

    public Dropdown frostedGlassLesionVolume;
    public Dropdown lungsVolume;
    public Dropdown changeStates;
    public Dropdown result;

    void Start()
    {
        text.text = "Добавить правило";
        applyButton.GetComponentInChildren<Text>().text = "Добавить";

        if (frostedGlassLesionVolume.options.Count > 0) frostedGlassLesionVolume.options.Clear();
        frostedGlassLesionVolume.AddOptions(KnowledgeBaseData.frostedGlassLesionVolumes);
        frostedGlassLesionVolume.value = 0;

        if (lungsVolume.options.Count > 0) lungsVolume.options.Clear();
        lungsVolume.AddOptions(KnowledgeBaseData.lungsVolumes);
        lungsVolume.value = 0;

        if (changeStates.options.Count > 0) changeStates.options.Clear();
        changeStates.AddOptions(KnowledgeBaseData.changeStates);
        changeStates.value = 0;

        if (result.options.Count > 0) result.options.Clear();
        result.AddOptions(KnowledgeBaseData.results);
        result.value = 0;
    }
}
