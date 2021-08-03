using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeriesFilter : Filter
{
    public InputField seriesName;
    public InputField description;

    public Button apply;
    public Button reset;

    void Start()
    {
        apply.onClick.AddListener(Filter);
        reset.onClick.AddListener(ResetFilter);
    }

    public void Filter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { "name", seriesName.text },
            { $"{DBTableNames.series}.description", description.text}
        };

        base.SetFilter(dictionary);
    }

    protected override void ResetFilter()
    {
        seriesName.text = "";
        description.text = "";

        base.ResetFilter();
    }
}
