using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CatchyClick;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UserResearchesData : TableData<UserResearchesData>
{
    public Authorization authorization;
    public Button update;

    private void Start()
    {
        update.onClick.AddListener(async () => await FillData());
    }
    public override async Task FillData()
    {
        if (authorization?.userData != null)
        {
            List<Research> researches = await DBResearches.GetResearches(filterQuery, $"{DBTableNames.users}.id");
            FillData(researches);
        }
        else
        {
            Debug.LogError("Необходимо авторизоваться");
        }

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
                new DataGridViewCell(Research.GetStringFromState(r.state))
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