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


        private static string HEADER = "header";
        private static string ROWS = "rows";

        [MenuItem("GameObject/UI/Custom/DataGridView")]
        private static void CreateInContextMenu()
        {
            GameObject canvas = new GameObject("Canvas");
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            GameObject dataGridView = new GameObject("DataGridView");
            dataGridView.transform.parent = canvas.transform;

            Image image = dataGridView.AddComponent<Image>();
            image.enabled = false;
            Color color = Color.white;
            color.a = 0.7f;
            image.color = color;

            RectTransform dgvRt = dataGridView.GetComponent<RectTransform>();
            dgvRt.anchorMin = Vector2.zero;
            dgvRt.anchorMax = Vector2.one;
            dgvRt.offsetMin = Vector2.zero;
            dgvRt.offsetMax = Vector3.zero;

            GameObject header = new GameObject(HEADER);
            header.transform.parent = dataGridView.transform;
            Image headerImage = header.AddComponent<Image>();
            headerImage.color = new Color(0.7f, 0.7f, 0.7f);

            RectTransform headerRt = header.GetComponent<RectTransform>();
            headerRt.anchorMin = new Vector2(0, 1);
            headerRt.anchorMax = Vector2.one;
            headerRt.pivot = new Vector2(0.5f, 1);
            headerRt.offsetMin = new Vector2(0f, -50f);
            headerRt.offsetMax = Vector2.zero;

            HorizontalLayoutGroup headerHLG = header.AddComponent<HorizontalLayoutGroup>();
            headerHLG.childAlignment = TextAnchor.MiddleLeft;
            headerHLG.childForceExpandWidth = false;
            headerHLG.childControlHeight = true;

            GameObject rows = new GameObject(ROWS);
            rows.transform.parent = dataGridView.transform;
            rows.AddComponent<Image>();

            RectTransform rowsRt = rows.GetComponent<RectTransform>();
            rowsRt.anchorMin = Vector2.zero;
            rowsRt.anchorMax = Vector2.one;
            rowsRt.pivot = new Vector2(0.5f, 1);
            rowsRt.offsetMin = Vector2.zero;
            rowsRt.offsetMax = new Vector2(0f, -50f);

            ScrollRect scrollRect = rows.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

            GameObject scrollObject = new GameObject("Scrollbar Vertical");
            Image scrollImage = scrollObject.AddComponent<Image>();
            scrollImage.color = new Color32(229, 229, 229, 255);

            RectTransform scrollRT = scrollObject.GetComponent<RectTransform>();
            scrollRT.SetParent(rowsRt);

            scrollRT.anchorMin = new Vector2(1, 0);
            scrollRT.anchorMax = Vector2.one;
            scrollRT.pivot = Vector2.one;
            scrollRT.offsetMax = Vector2.zero;
            scrollRT.offsetMin = new Vector2(-20, -17);

            Scrollbar scrollbar = scrollObject.AddComponent<Scrollbar>();
            scrollRect.verticalScrollbar = scrollbar;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;

            GameObject slidingArea = new GameObject("Sliding Area");
            RectTransform slidingAreaRT = slidingArea.AddComponent<RectTransform>();
            slidingAreaRT.SetParent(scrollRT);
            slidingAreaRT.anchorMin = Vector2.zero;
            slidingAreaRT.anchorMax = Vector2.one;
            slidingAreaRT.offsetMin = new Vector2(10, 10);
            slidingAreaRT.offsetMax = new Vector2(-10, -10);

            GameObject handleObject = new GameObject("Handle");
            Image handleImage = handleObject.AddComponent<Image>();
            handleImage.color = new Color32(180, 180, 180, 255);
            RectTransform handleRT = handleObject.GetComponent<RectTransform>();
            scrollbar.handleRect = handleRT;
            handleRT.SetParent(slidingAreaRT);
            handleRT.anchorMin = new Vector2(0, 1);
            handleRT.anchorMax = Vector2.one;
            handleRT.offsetMin = new Vector2(-10, 10);
            handleRT.offsetMax = new Vector2(10, -10);

            GameObject viewport = new GameObject("Viewport");
            Image viewportImage = viewport.AddComponent<Image>();

            RectTransform viewportRT = viewport.GetComponent<RectTransform>();
            viewportRT.SetParent(rowsRt);

            scrollRect.viewport = viewportRT;

            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.offsetMin = Vector2.zero;
            viewportRT.offsetMax = Vector2.zero;

            viewport.AddComponent<Mask>();

            GameObject content = new GameObject("Content");
            content.transform.parent = viewport.transform;

            RectTransform contentRT = content.AddComponent<RectTransform>();
            scrollRect.content = contentRT;
            contentRT.pivot = new Vector2(0.5f, 1);
            contentRT.anchorMin = new Vector2(0f, 1f);
            contentRT.anchorMax = Vector2.one;
            contentRT.offsetMin = Vector2.zero;
            contentRT.offsetMax = Vector2.zero;

            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 1;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;

            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            DataGridView dgv = dataGridView.AddComponent<DataGridView>();
            dgv.rows.changed.AddListener(dgv.OnChange);
            dgv.headerComponent = header;
            dgv.rowsComponent = content;
        }

        private void Awake()
        {
            _GetHeaderElements();

            rows.changed.AddListener(OnChange);
        }

        private void _GetHeaderElements()
        {
            DataGridViewHeaderElement[] headers = headerComponent.GetComponentsInChildren<DataGridViewHeaderElement>();
            if (headers.Length > 0)
            {
                this.headers = headers.ToList();
            }
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
                throw new System.Exception("Количество ячеек в строке не соответствует заголовку!");
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