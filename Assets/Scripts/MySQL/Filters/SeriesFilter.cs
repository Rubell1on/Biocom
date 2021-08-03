using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeriesFilter : MonoBehaviour
{
    public InputField seriesName;
    public InputField description;

    public Button apply;
    public Button reset;

    public SeriesData seriesData;

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

        QueryBuilder queryBuilder = new QueryBuilder(dictionary);
        List<Series> users = DBSeries.GetSeries(queryBuilder);
        seriesData.FillData(users);
    }

    public void ResetFilter()
    {
        seriesName.text = "";
        description.text = "";

        seriesData.FillData();
    }
}
