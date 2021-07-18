using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CatchyClick;

public class ResearchesData : TableData<ResearchesData>
{
    public override void FillData()
    {
        List<Research> researches = DBResearches.GetResearches();

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
                new DataGridViewCell(r.seriesName.ToString())
            };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
            dataGridView.rows.Clear();

        dataGridView.rows.AddRange(rows);
    }
}