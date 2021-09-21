using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;
public class KnowledgeBaseData : TableData<KnowledgeBaseData>
{
    public static List<string> frostedGlassLesionVolume = new List<string>() { "�����������", "������", "�������", "�������" };
    public static List<string> lungsVolume = new List<string>() { "�����", "�������", "�������" };
    public static List<string> changeStates = new List<string>() { "�����������", "������", "�������", "�������" };
    public static List<string> result1 = new List<string>() { "�����������", "������", "�������", "�������" };

    public override async Task FillData()
    {
        List<Knowledge> knowledgeBase = new List<Knowledge>();

        for (int i = 0, id = 0; i < frostedGlassLesionVolume.Count; i++)
        {
            for (int j = 0; j < lungsVolume.Count; j++)
            {
                for (int x = 0; x < changeStates.Count; x++)
                {
                    for (int y = 0; y < changeStates.Count; y++)
                    {
                        string rule = $"IF (�����_���������_�������_������� IS {frostedGlassLesionVolume[i]}) AND (�����_������ IS {lungsVolume[j]}) AND (�������_��������� IS {changeStates[x]}) THEN (��������� IS {result1[y]})";
                        knowledgeBase.Add(new Knowledge(id, rule));
                        id += 1;
                    }
                }
            }
        }

        List<DataGridViewRow> rows = knowledgeBase.Select(k =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(k.id.ToString()),
                new DataGridViewCell(k.rule)
            };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
        {
            dataGridView.ClearRows();
            dataGridView.rows.Clear();
        }

        dataGridView.rows.AddRange(rows);

        return;
    }

    //public void FillData(List<User> users)
    //{
    //    List<DataGridViewRow> rows = users.Select(u =>
    //    {
    //        List<DataGridViewCell> cells = new List<DataGridViewCell>()
    //        {
    //            new DataGridViewCell(u.id.ToString()),
    //            new DataGridViewCell(u.userName),
    //            new DataGridViewCell(u.role.ToString())
    //        };

    //        return new DataGridViewRow(cells);
    //    }).ToList();

    //    if (dataGridView.rows.Count > 0)
    //    {
    //        dataGridView.ClearRows();
    //        dataGridView.rows.Clear();
    //    }

    //    dataGridView.rows.AddRange(rows);
    //}
}

public class Knowledge
{
    public int id;
    public string rule;

    public Knowledge(int id, string rule)
    {
        this.id = id;
        this.rule = rule;
    }
}