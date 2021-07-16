using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class PartsData : TableData<PartsData>
{
    public override void FillData()
    {
        List<Part> series = DBParts.GetParts();

        List<DataGridViewRow> rows = series.Select(u =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(u.id.ToString()),
                new DataGridViewCell(u.researchId.ToString()),
                new DataGridViewCell(u.remoteId.ToString()),
                new DataGridViewCell(u.filePath.ToString())
            };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
            dataGridView.rows.Clear();

        dataGridView.rows.AddRange(rows);
    }
}