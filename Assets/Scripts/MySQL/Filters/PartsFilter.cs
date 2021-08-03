using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsFilter : Filter
{
    public InputField partPath;
    public InputField seriesName;

    public Button apply;
    public Button reset;

    public PartsData partsData;

    void Start()
    {
        apply.onClick.AddListener(Filter);
        reset.onClick.AddListener(ResetFilter);
    }

    public void Filter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { $"{DBTableNames.parts}.filePath", partPath.text },
            { $"{DBTableNames.series}.name", seriesName.text }
        };

        base.SetFilter(dictionary);
    }

    protected override void ResetFilter()
    {
        partPath.text = "";
        seriesName.text = "";

        base.ResetFilter();
    }
}
