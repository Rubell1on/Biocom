using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class DGVTest : MonoBehaviour
{
    CatchyClick.DataGridView gridView;
    // Start is called before the first frame update
    void Start()
    {
        gridView = GetComponent<CatchyClick.DataGridView>();
        gridView.cellClicked.AddListener(d => Debug.Log($"row: {d.row} cell: {d.cell}"));

        string[] userNames = new string[]
        {
            "Хохлов Алексей Аркадьевич",
            "Метелина Елена Сергеевна",
            "Сбитнева Анастасия Андреевна",
            "Аблаев Дмитрий Алексеевич",
            "Аалаев Дмитрий Алексеевич"
        };

        List<DataGridViewRow> rows = new List<DataGridViewRow>();
        for (int i = 0; i < userNames.Length; i++)
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>
            {
                new DataGridViewCell(i.ToString()),
                new DataGridViewCell(userNames[i]),
                new DataGridViewCell("Role"),
                new DataGridViewCell("BirthDate")
            };

            rows.Add(new DataGridViewRow(cells));  
        }

        gridView.rows.AddRange(rows);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
