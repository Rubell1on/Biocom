using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;

public class PartsData : TableData<PartsData>
{
    public override async Task FillData()
    {
        List<Part> parts = await DBParts.GetParts(filterQuery);
        FillData(parts);

        return;
    }

    public void FillData(List<Part> parts)
    {
        List<DataGridViewRow> rows = parts.Select(u =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(u.id.ToString()),
                new DataGridViewCell(u.seriesId.ToString()),
                new DataGridViewCell(u.remoteId.ToString()),
                new DataGridViewCell(u.filePath.ToString()),
                new DataGridViewCell(u.seriesName.ToString()),
                new DataGridViewCell(u.tissue.rusName)
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