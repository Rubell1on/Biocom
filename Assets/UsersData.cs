using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class UsersData : MonoBehaviour
{
    DataGridView dataGridView;
    public DataGridViewRow selectedRow;

    void Start()
    {
        dataGridView = GetComponent<DataGridView>();
        dataGridView.cellClicked.AddListener(OnCellClicked);
        FillUserData();
    }

    private void OnCellClicked(DataGridViewEventArgs e)
    {
        selectedRow = dataGridView.rows[e.row];
    }

    public void FillUserData()
    {
        List<User> users = DBUsers.GetUsers();

        List<DataGridViewRow> rows = users.Select(u =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(u.id.ToString()),
                new DataGridViewCell(u.userName),
                new DataGridViewCell(u.role.ToString())
            };

            return new DataGridViewRow(cells);
        }).ToList();

        if (dataGridView.rows.Count > 0)
            dataGridView.rows.Clear();

        dataGridView.rows.AddRange(rows);
    }
}
