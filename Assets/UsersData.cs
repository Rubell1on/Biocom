using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class UsersData : MonoBehaviour
{
    DataGridView dataGridView;
    void Start()
    {
        dataGridView = GetComponent<DataGridView>();
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

        dataGridView.rows.AddRange(rows);
    }
}
