using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CatchyClick
{
    [RequireComponent(typeof(DataGridView))]
    public class DataGridViewRowSelector : MonoBehaviour
    {
        public Color32 selectedColor = new Color32(149, 149, 149, 255);
        public DataGridViewRowUI selectedRow;
        public UnityEvent<DataGridViewRowUI> rowSelectionChanged = new UnityEvent<DataGridViewRowUI>();
        private DataGridView dataGrid;
        private Color32 color;

        void Start()
        {
            dataGrid = GetComponent<DataGridView>();
            dataGrid.cellClicked.AddListener(OnCellClicked);
        }
        private void OnDisable()
        {
            selectedRow = null;
        }

        void OnCellClicked(DataGridViewEventArgs args)
        {
            if (selectedRow != null)
            {
                selectedRow.cells.ForEach(c => c.GetComponent<Image>().color = color);
            }

            selectedRow = dataGrid.uiRows[args.row];

            DataGridViewCellUI cellUI = selectedRow.cells[args.cell];
            if (cellUI != null)
            {
                Image cellImage = cellUI.GetComponent<Image>();
                color = cellImage.color;
                
                selectedRow.cells.ForEach(c => c.GetComponent<Image>().color = selectedColor);
                rowSelectionChanged.Invoke(selectedRow);
            }
        }
    }
}