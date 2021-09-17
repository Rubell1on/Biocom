using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;

public class SeriesData : TableData<SeriesData>
{
    public override async Task FillData()
    {
        List<Series> series = await DBSeries.GetSeries(filterQuery);
        FillData(series);

        return;
    }

    public void FillData(List<Series> series)
    {
        List<DataGridViewRow> rows = series.Select(u =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(u.id.ToString()),
                new DataGridViewCell(u.name.ToString()),
                new DataGridViewCell(u.description.ToString()),
                new DataGridViewCell(u.researchId.ToString()),
                new DataGridViewCell(u.sourceNiiFilePath.ToString())
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