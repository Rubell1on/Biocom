using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsFilter : MonoBehaviour
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

        QueryBuilder queryBuilder = new QueryBuilder(dictionary);
        List<Part> parts = DBParts.GetParts(queryBuilder);
        partsData.FillData(parts);
    }

    public void ResetFilter()
    {
        partPath.text = "";
        seriesName.text = "";
        partsData.FillData();
    }
}
