using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatchyClick;
using UnityEngine;

public class TissuesData : TableData<TissuesData>
{
    public override void FillData()
    {
        List<Tissue> tissues = DBTissues.GetTissues(filterQuery);
        FillData(tissues);
    }

    public void FillData(List<Tissue> tissues)
    {
        List<DataGridViewRow> rows = tissues.Select(t =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(t.id.ToString()),
                new DataGridViewCell(t.name.ToString()),
                new DataGridViewCell(t.rusName.ToString()),
                new DataGridViewCell(ColorUtility.ToHtmlStringRGBA(t.color))
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