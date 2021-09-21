using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;
public class KnowledgeBaseData : TableData<KnowledgeBaseData>
{
    public static List<string> frostedGlassLesionVolume = new List<string>() { "Отсутствует", "Низкий", "Средний", "Высокий" };
    public static List<string> lungsVolume = new List<string>() { "Малый", "Средний", "Большой" };
    public static List<string> changeStates = new List<string>() { "Отсутствует", "Низкая", "Средняя", "Высокая" };
    public static List<string> result1 = new List<string>() { "Отсутствует", "Низкий", "Средний", "Высокий" };

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
                        string rule = $"IF (Объем_поражения_матовым_стеклом IS {frostedGlassLesionVolume[i]}) AND (Объем_легких IS {lungsVolume[j]}) AND (Степень_изменений IS {changeStates[x]}) THEN (Результат IS {result1[y]})";
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