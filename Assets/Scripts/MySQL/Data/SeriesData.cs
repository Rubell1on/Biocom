using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class SeriesData : TableData<SeriesData>
{
    public override void FillData()
    {
        List<Series> series = DBSeries.GetSeries();

        List<DataGridViewRow> rows = series.Select(u =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(u.id.ToString()),
                new DataGridViewCell(u.name.ToString()),
                new DataGridViewCell(u.description.ToString()),
                new DataGridViewCell(u.researchId.ToString())
            };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
            dataGridView.rows.Clear();

        dataGridView.rows.AddRange(rows);
    }
}