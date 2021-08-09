﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CatchyClick;
using UnityEngine;

public class UserResearchesData : TableData<UserResearchesData>
{
    public Authorization authorization;

    public override void FillData()
    {
        if (authorization?.userData != null)
        {
            List<Research> researches = DBResearches.GetResearches(filterQuery, $"{DBTableNames.users}.id");
            FillData(researches);
        }
        else
        {
            Debug.LogError("Необходимо авторизоваться");
        }
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
        {
            dataGridView.ClearRows();
            dataGridView.rows.Clear();
        }

        dataGridView.rows.AddRange(rows);
    }
}