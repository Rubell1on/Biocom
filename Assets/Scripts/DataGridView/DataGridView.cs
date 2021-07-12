using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DataGridView {
    public class DataGridView : MonoBehaviour
    {
        public GameObject headerComponent;
        public GameObject rowsComponent;

        [SerializeField]
        public List<DataGridViewHeaderElement> header;
        [SerializeField]
        private List<DataGridViewRow> rows = new List<DataGridViewRow>();
    
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

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.parent = rows.transform;

            Image viewportImage = viewport.AddComponent<Image>();

            RectTransform viewportRT = viewport.GetComponent<RectTransform>();
            //viewportRT.pivot = new Vector2(0, 1);
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.offsetMin = Vector2.zero;
            viewportRT.offsetMax = Vector2.zero;

            viewport.AddComponent<Mask>();

            GameObject content = new GameObject("Content");
            content.transform.parent = viewport.transform;

            RectTransform contentRT = content.AddComponent<RectTransform>();
            scrollRect.content = contentRT;
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
            dgv.headerComponent = header;
            dgv.rowsComponent = rows;
        }

        public void AddRows(List<DataGridViewRow> rows)
        {
            this.rows = rows;
            List<DataGridViewRowUI> uiRows = new List<DataGridViewRowUI>();
            rows.ForEach(r =>
            {
                GameObject row = DataGridViewRowUI.CreateRow();
                DataGridViewRowUI rowUI = row.GetComponent<DataGridViewRowUI>();

                r.cells.ForEach(c =>
                {
                    GameObject cell = DataGridViewCellUI.CreateCell();
                    cell.transform.SetParent(rowUI.transform);
                    DataGridViewCellUI cellUI = cell.GetComponent<DataGridViewCellUI>();
                    cellUI.textComponent.text = c.value;
                    rowUI.cells.Add(cellUI);
                });

                uiRows.Add(rowUI);
            });

            uiRows.ForEach(r =>
            {
                r.transform.SetParent(rowsComponent.transform);
                RectTransform rowRT = r.GetComponent<RectTransform>();
                rowRT.anchoredPosition3D = Vector3.zero;
                rowRT.localScale = Vector3.one;
            });
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}