using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataGridView;

public class DGVTest : MonoBehaviour
{
    DataGridView.DataGridView gridView;
    // Start is called before the first frame update
    void Start()
    {
        gridView = GetComponent<DataGridView.DataGridView>();
        gridView.cellClicked.AddListener(d => Debug.Log($"row: {d.row} cell: {d.cell}"));
        List<DataGridViewRow> rows = new List<DataGridViewRow>();
        for (int i = 0; i < 40; i++)
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>
            {
                new DataGridViewCell(i.ToString()),
                new DataGridViewCell("UserName"),
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
