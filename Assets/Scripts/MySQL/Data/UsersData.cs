using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class UsersData : TableData<UsersData>
{
    public override void FillData()
    {
        List<User> users = DBUsers.GetUsers();
        FillData(users);
    }

    public void FillData(List<User> users)
    {
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
