using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CatchyClick;
using UnityEngine;

public class UserResearchesData : TableData<UserResearchesData>
{
    public Authorization authorization;
    public Filter filter;

    public QueryBuilder filterQuery;

    public void Start()
    {
        filterQuery = filter.query;
        filter.filterChanged.AddListener(OnFilterChanged);
    }

    public override void FillData()
    {
        if (authorization?.userData != null)
        {
            List<Research> researches = DBResearches.GetResearches(filterQuery);
            FillData(researches);
        }
        else
        {
            Debug.LogError("Необходимо авторизоваться");
        }
    }

    public void OnFilterChanged(QueryBuilder queryBuilder)
    {
        this.filterQuery = queryBuilder;
        FillData();
    }

    public void FillData(List<Research> researches)
    {
        List<DataGridViewRow> rows = researches.Select(r =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
                {
                new DataGridViewCell(r.id.ToString()),
                new DataGridViewCell(r.date.ToString()),
                new DataGridViewCell(r.description.ToString()),
                new DataGridViewCell(r.note.ToString()),
                new DataGridViewCell(r.state.ToString()),
                new DataGridViewCell(r.userName.ToString()),
                };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
            dataGridView.ClearRows();

        dataGridView.rows.AddRange(rows);
    }
}