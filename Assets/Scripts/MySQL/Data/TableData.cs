using UnityEngine;
using CatchyClick;

public abstract class TableData<T> : MonoBehaviour
{
    public DataGridView dataGridView;
    public DataGridViewRow selectedRow;

    void Start()
    {
        dataGridView = GetComponent<DataGridView>();
        dataGridView.cellClicked.AddListener(OnCellClicked);
        FillData();
    }

    private void OnCellClicked(DataGridViewEventArgs e)
    {
        selectedRow = dataGridView.rows[e.row];
    }

    public abstract void FillData();
}