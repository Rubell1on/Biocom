using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DataGridView {
    public class DataGridView : MonoBehaviour
    {
        [Header("Root components")]
        public GameObject headerComponent;
        public GameObject rowsComponent;
        [Space(10)]

        [Header("Data lists")]
        [SerializeField]
        public List<DataGridViewHeaderElement> headers;
        [SerializeField]
        public CustomList<DataGridViewRow> rows = new CustomList<DataGridViewRow>();
        List<DataGridViewRowUI> uiRows = new List<DataGridViewRowUI>();
        [Space(10)]

        [Header("Row colors")]
        public bool colorRows = true;
        public Color even = Color.white;
        public Color odd = new Color32(174, 174, 174, 255);
        [Space(10)]

        [Header("Events")]
        public DataGridViewCellClickEvent cellClicked = new DataGridViewCellClickEvent();

        private List<bool> sortings = new List<bool>();

        private void Awake()
        {
            headers = _GetHeaderElements();

            if (sortings.Count > 0)
            {
                sortings.Clear();
            }

            headers.ForEach(h => sortings.Add(false));

            for (int i = 0; i < headers.Count; i++)
            {
                DataGridViewHeaderElement header = headers[i];
                Button headerButton = header.GetComponent<Button>();
                int id = i;
                headerButton.onClick.AddListener(() =>
                {
                    bool sorting = sortings[id];
                    if (sorting)
                    {
                        rows.Sort((a, b) => a.cells[id].value.CompareTo(b.cells[id].value));
                    } 
                    else
                    {
                        rows.Sort((a, b) => a.cells[id].value.CompareTo(b.cells[id].value));
                        rows.Reverse();
                    }

                    sortings[id] = sortings[id] ? false : true;

                    rows.changed?.Invoke();
                });
            }

            rows.changed.AddListener(OnChange);
        }

        private List<DataGridViewHeaderElement> _GetHeaderElements()
        {
            DataGridViewHeaderElement[] headers = headerComponent.GetComponentsInChildren<DataGridViewHeaderElement>();
            if (headers.Length > 0)
            {
                return headers.ToList();
            }

            return null;
        }

        public void OnChange()
        {
            if (rows?[0].cells.Count == headers?.Count)
            {
                if (uiRows.Count > 0)
                {
                    uiRows.ForEach(r => Destroy(r));
                    uiRows.Clear();
                }

                for(int i = 0; i < rows.Count; i++)
                {
                    DataGridViewRow row = rows[i];
                    GameObject rowObject = DataGridViewRowUI.CreateRow();
                    DataGridViewRowUI rowUI = rowObject.GetComponent<DataGridViewRowUI>();

                    Image rowImage = rowObject.GetComponent<Image>();
                    if (colorRows)
                    {
                        rowImage = rowObject.GetComponent<Image>();
                        rowImage.color = i == 0 || i % 2 == 0 ? even : odd;
                    } else
                    {
                        rowImage.color = even;
                    }

                    for (int j = 0; j < row.cells.Count; j++)
                    {

                        GameObject cell = DataGridViewCellUI.CreateCell();

                        RectTransform cellRT = cell.GetComponent<RectTransform>();
                        RectTransform headerRT = headers[j].GetComponent<RectTransform>();
                        cellRT.sizeDelta = new Vector2(headerRT.sizeDelta.x, cellRT.sizeDelta.y);

                        cell.transform.SetParent(rowUI.transform);
                        DataGridViewCellUI cellUI = cell.GetComponent<DataGridViewCellUI>();
                        cellUI.textComponent.text = row.cells[j].value;

                        Button cellButton = cell.GetComponent<Button>();
                        DataGridViewEventArgs args = new DataGridViewEventArgs(i, j);
                        cellButton.onClick.AddListener(OnClick);

                        rowUI.cells.Add(cellUI);

                        void OnClick()
                        {
                            cellClicked.Invoke(args);
                        }
                    }

                    uiRows.Add(rowUI);
                }

                uiRows.ForEach(r =>
                {
                    r.transform.SetParent(rowsComponent.transform);
                    RectTransform rowRT = r.GetComponent<RectTransform>();
                    rowRT.anchoredPosition3D = Vector3.zero;
                    rowRT.localScale = Vector3.one;
                });

            } else
            {
                throw new System.Exception("���������� ����� � ������ �� ������������� ���������!");
            }
        }
    }

    [Serializable]
    public class DataGridViewEventArgs
    {
        public int row;
        public int cell;

        public DataGridViewEventArgs(int row, int cell)
        {
            this.row = row;
            this.cell = cell;
        }
    }

    [Serializable]
    public class DataGridViewCellClickEvent : UnityEvent<DataGridViewEventArgs> { };
}