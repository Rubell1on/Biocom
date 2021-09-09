using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;

public abstract class TableData<T> : MonoBehaviour
{
    public DataGridView dataGridView;
    public DataGridViewRow selectedRow;

    public QueryBuilder filterQuery = new QueryBuilder(new Dictionary<string, string>());

    async void OnEnable()
    {
        dataGridView = GetComponent<DataGridView>();
        dataGridView.cellClicked.AddListener(OnCellClicked);
        await FillData();
    }

    private void OnDisable()
    {
        dataGridView.cellClicked.RemoveListener(OnCellClicked);
    }

    private void OnCellClicked(DataGridViewEventArgs e)
    {
        selectedRow = dataGridView.rows[e.row];
    }

    public abstract Task FillData();

    public async void OnFilterChanged(QueryBuilder queryBuilder)
    {
        this.filterQuery = queryBuilder;
        await FillData();
    }
}