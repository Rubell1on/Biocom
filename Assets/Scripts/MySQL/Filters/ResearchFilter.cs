using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ResearchFilter : Filter
{
    public InputField description;
    public InputField note;
    public InputField date;
    public Dropdown state;

    public Button apply;
    public Button reset;

    private void Start()
    {
        if (state.options.Count > 0) state.ClearOptions();
        List<string> states = Enum.GetNames(typeof(Research.State)).ToList();
        states.Insert(0, "Все");
        state.AddOptions(states);

        apply.onClick.AddListener(SetFilter);
        reset.onClick.AddListener(ResetFilter);
    }

    public void SetFilter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { "description", description.text },
            { "note", note.text },
            { "date", date.text},
            { "state", state.value != 0 ? state.options[state.value].text : ""}
        };

        base.SetFilter(dictionary);
    }

    protected override void ResetFilter()
    {
        description.text = "";
        note.text = "";
        date.text = "";
        state.value = 0;

        base.ResetFilter();
    }
}