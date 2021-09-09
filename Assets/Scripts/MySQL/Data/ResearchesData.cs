using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CatchyClick;
using System.Threading.Tasks;

public class ResearchesData : TableData<ResearchesData>
{
    public override async Task FillData()
    {
        List<Research> researches = await DBResearches.GetResearches(filterQuery);
        FillData(researches);

        return;
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