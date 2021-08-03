using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public abstract class TableData<T> : MonoBehaviour
{
    public DataGridView dataGridView;
    public DataGridViewRow selectedRow;

    public QueryBuilder filterQuery = new QueryBuilder(new Dictionary<string, string>());

    void OnEnable()
    {
        dataGridView = GetComponent<DataGridView>();
        dataGridView.cellClicked.AddListener(OnCellClicked);
        FillData();
    }

    private void OnDisable()
    {
        dataGridView.cellClicked.RemoveListener(OnCellClicked);
    }

    private void OnCellClicked(DataGridViewEventArgs e)
    {
        selectedRow = dataGridView.rows[e.row];
    }

    public abstract void FillData();

    public void OnFilterChanged(QueryBuilder queryBuilder)
    {
        this.filterQuery = queryBuilder;
        FillData();
    }
}