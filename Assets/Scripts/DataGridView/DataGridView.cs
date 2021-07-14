using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;

namespace CatchyClick {
    public class DataGridView : MonoBehaviour
    {
        [Header("Root components")]
        public GameObject headerComponent;
        public GameObject rowsComponent;
        [Space(10)]

        [Header("Data lists")]
        [SerializeField]
        public List<DataGridViewHeaderElementUI> headers;
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
                DataGridViewHeaderElementUI header = headers[i];
                Button headerButton = header.GetComponent<Button>();
                int id = i;
                headerButton.onClick.AddListener(() =>
                {
                    bool sorting = sortings[id];

                    rows.Sort((a, b) => {
                        string firstString = a.cells[id].value;
                        string secondString = b.cells[id].value;

                        int first;
                        int second;

                        if (Int32.TryParse(firstString, out first) && Int32.TryParse(secondString, out second))
                        {
                            return first.CompareTo(second);
                        }

                        return firstString.CompareTo(secondString);
                    });


                    if (sorting)
                    {
                        rows.Reverse();
                    }

                    sortings = sortings.Select(x => false).ToList();
                    sortings[id] = sorting ? false : true;

                    rows.changed?.Invoke();
                });
            }

            rows.changed.AddListener(OnChange);
        }

        private List<DataGridViewHeaderElementUI> _GetHeaderElements()
        {
            DataGridViewHeaderElementUI[] headers = headerComponent.GetComponentsInChildren<DataGridViewHeaderElementUI>();
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
                    uiRows.ForEach(r => Destroy(r.gameObject));
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
                throw new System.Exception("Cells count don't match with header!");
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

    public static class StringExtention
    {
        public static int GetByteSumm(this string input)
        {
            return Encoding.UTF8.GetBytes(input).ToList().Aggregate(0, (acc, value) => acc + value);
        }
    }
}