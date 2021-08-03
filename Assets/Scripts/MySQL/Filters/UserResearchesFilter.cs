using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class UserResearchesFilter : Filter
{
    public InputField description;
    public InputField note;
    public InputField date;
    public Dropdown state;

    public Button apply;
    public Button reset;

    public Authorization authorization;

    private void Start()
    {
        if (state.options.Count > 0) state.ClearOptions();
        List<string> states = Enum.GetNames(typeof(Research.State)).ToList();
        states.Insert(0, "Все");
        state.AddOptions(states);

        apply.onClick.AddListener(SetFilter);
        reset.onClick.AddListener(ResetFilter);
    }

    void OnEnable()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.users}.id", authorization.userData.id.ToString() } 
        };

        this.defaultQuery = new QueryBuilder(dictionary);

        SetFilter(dictionary);
    }

    private void SetFilter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>(defaultQuery.dictionary)
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