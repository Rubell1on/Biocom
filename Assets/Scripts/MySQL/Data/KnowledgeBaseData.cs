using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;
public class KnowledgeBaseData : TableData<KnowledgeBaseData>
{
    public static List<string> frostedGlassLesionVolumes = new List<string>() { "�����������", "������", "�������", "�������" };
    public static List<string> lungsVolumes = new List<string>() { "�����", "�������", "�������" };
    public static List<string> changeStates = new List<string>() { "�����������", "������", "�������", "�������" };
    public static List<string> temperatures = new List<string>() { "����������", "����������", "�������" };
    public static List<string> bloodOxygenSaturations = new List<string>() { "������", "����������", "����������" };
    public static List<string> frostedGlassCounts = new List<string>() { "���������", "�������", "�������" };
    public static List<string> comorbidities = new List<string>() { "���", "������", "�������" };
    public static List<string> ages = new List<string>() { "�������", "�������", "�������", "����������" };
    public static List<string> results = new List<string>() { "�����������", "������", "�������", "�������" };

    public override async Task FillData()
    {
        List<Knowledge> knowledgeBase = new List<Knowledge>();

        //for (int i = 0, id = 0; i < frostedGlassLesionVolumes.Count; i++)
        //{
        //    for (int j = 0; j < lungsVolumes.Count; j++)
        //    {
        //        for (int x = 0; x < changeStates.Count; x++)
        //        {
        //            for (int y = 0; y < results.Count; y++)
        //            {
        //                string rule = $"IF (�����_���������_�������_������� IS {frostedGlassLesionVolumes[i]}) AND (�����_������ IS {lungsVolumes[j]}) AND (�������_��������� IS {changeStates[x]}) THEN (��������� IS {results[y]})";
        //                knowledgeBase.Add(new Knowledge(id, rule));
        //                id += 1;
        //            }
        //        }
        //    }
        //}
        string rulesFilePath = $"{Application.dataPath}/rules.txt";
        if (!File.Exists(rulesFilePath))
        {
            int id = 0;

            frostedGlassLesionVolumes.ForEach(frostedGlassVolume =>
            {
                lungsVolumes.ForEach(lungsVolume =>
                {
                    changeStates.ForEach(change =>
                    {
                        temperatures.ForEach(temperature =>
                        {
                            bloodOxygenSaturations.ForEach(saturation =>
                            {
                                frostedGlassCounts.ForEach(frostedGlassCount =>
                                {
                                    comorbidities.ForEach(comorbidity =>
                                    {
                                        ages.ForEach(age =>
                                        {
                                            results.ForEach(result =>
                                            {
                                                string rule = $"IF (�����_���������_�������_������� IS {frostedGlassVolume}) " +
                                                    $"AND (�����_������ IS {lungsVolume}) " +
                                                    $"AND (�������_��������� IS {change}) " +
                                                    $"AND (����������� IS {temperature}) " +
                                                    $"AND (���������_���������_����� IS {saturation}) " +
                                                    $"AND (����������_��������_��������_������) IS {frostedGlassCount} " +
                                                    $"AND (�������������_��������� IS {comorbidity}) " +
                                                    $"AND (������� IS {age} " +
                                                    $"THEN (��������� IS {result})";
                                                knowledgeBase.Add(new Knowledge(id, rule));
                                                id++;
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });

            int ruleId = 1;
            string data = knowledgeBase.Aggregate("", (acc, curr) =>
            {
                acc += $"{ruleId}. {curr.rule}\n";
                ruleId++;
                return acc;
            });
            File.WriteAllText(rulesFilePath, data);
        }
        else
        {
            List<string> rules = File.ReadAllLines(rulesFilePath).Take(100).ToList();
            rules.ForEach(r =>
            {
                string[] splittedString = r.Split(new string[] { ". " }, System.StringSplitOptions.None);
                int id = Int32.Parse(splittedString[0]);
                knowledgeBase.Add(new Knowledge(id, splittedString[1]));
            });
            Debug.Log("");
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