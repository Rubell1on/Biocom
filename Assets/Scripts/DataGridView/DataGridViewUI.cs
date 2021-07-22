using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace CatchyClick
{
    public class DataGridViewUI
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Custom/DataGridView", priority = 0)]
        private static void CreateInContextMenu()
        {
            GameObject dataGridView = new GameObject("DataGridView");

            if (Selection.activeTransform == null)
            {
                GameObject canvas = new GameObject("Canvas");
                canvas.AddComponent<Canvas>();
                canvas.AddComponent<CanvasScaler>();
                canvas.AddComponent<GraphicRaycaster>();
                dataGridView.transform.parent = canvas.transform;
            } else
            {
                dataGridView.transform.parent = Selection.activeTransform;
            }

            dataGridView.AddComponent<DataGridViewRowSelector>();
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
            dgvRt.localScale = Vector3.one;
            dgvRt.anchoredPosition3D = Vector3.zero;

            GameObject header = new GameObject("Columns");
            Image headerImage = header.AddComponent<Image>();
            headerImage.color = new Color(0.7f, 0.7f, 0.7f);

            RectTransform headerRT = header.GetComponent<RectTransform>();
            headerRT.SetParent(dgvRt);
            headerRT.localScale = Vector3.one;
            headerRT.anchorMin = new Vector2(0, 1);
            headerRT.anchorMax = Vector2.one;
            headerRT.pivot = new Vector2(0.5f, 1);
            headerRT.anchoredPosition3D = Vector3.zero;
            headerRT.offsetMin = new Vector2(0f, -50f);
            headerRT.offsetMax = Vector2.zero;

            HorizontalLayoutGroup headerHLG = header.AddComponent<HorizontalLayoutGroup>();
            headerHLG.childAlignment = TextAnchor.MiddleLeft;
            headerHLG.childForceExpandWidth = false;
            headerHLG.childControlHeight = true;
            headerHLG.spacing = 1;

            GameObject scrollObject = new GameObject("Scrollbar Vertical");
            Image scrollImage = scrollObject.AddComponent<Image>();
            scrollImage.color = new Color32(229, 229, 229, 255);

            RectTransform scrollRT = scrollObject.GetComponent<RectTransform>();

            Scrollbar scrollbar = scrollObject.AddComponent<Scrollbar>();
            
            scrollbar.direction = Scrollbar.Direction.BottomToTop;

            GameObject slidingArea = new GameObject("Sliding Area");
            RectTransform slidingAreaRT = slidingArea.AddComponent<RectTransform>();
            slidingAreaRT.SetParent(scrollRT);
            slidingAreaRT.localScale = Vector3.one;
            slidingAreaRT.anchorMin = Vector2.zero;
            slidingAreaRT.anchorMax = Vector2.one;
            slidingAreaRT.anchoredPosition3D = Vector3.zero;
            slidingAreaRT.offsetMin = new Vector2(10, 0);
            slidingAreaRT.offsetMax = new Vector2(-10, 0);

            GameObject handleObject = new GameObject("Handle");
            Image handleImage = handleObject.AddComponent<Image>();
            handleImage.color = new Color32(180, 180, 180, 255);
            RectTransform handleRT = handleObject.GetComponent<RectTransform>();
            scrollbar.handleRect = handleRT;
            handleRT.SetParent(slidingAreaRT);
            handleRT.localScale = Vector3.one;
            handleRT.anchorMin = new Vector2(0, 1);
            handleRT.anchorMax = Vector2.one;
            handleRT.anchoredPosition3D = new Vector3(5, 0, 0);
            handleRT.offsetMin = new Vector2(-10, 0);
            handleRT.offsetMax = new Vector2(10, 0);

            GameObject rows = new GameObject("Rows");
            rows.AddComponent<Mask>();
            rows.AddComponent<Image>();

            RectTransform rowsRt = rows.GetComponent<RectTransform>();
            rowsRt.SetParent(dgvRt);
            rowsRt.localScale = Vector3.one;
            rowsRt.anchorMin = Vector2.zero;
            rowsRt.anchorMax = Vector2.one;
            rowsRt.pivot = new Vector2(0.5f, 1);
            rowsRt.anchoredPosition3D = Vector3.zero;
            rowsRt.offsetMin = Vector2.zero;
            rowsRt.offsetMax = new Vector2(0f, -50f);

            ScrollRect scrollRect = rows.AddComponent<ScrollRect>();
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.scrollSensitivity = 20;
            scrollRect.horizontal = false;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarSpacing = -20;

            scrollRT.SetParent(rowsRt);

            scrollRT.localScale = Vector3.one;
            scrollRT.anchorMin = new Vector2(1, 0);
            scrollRT.anchorMax = Vector2.one;
            scrollRT.pivot = Vector2.one;
            scrollRT.anchoredPosition3D = Vector3.zero;
            scrollRT.offsetMax = Vector2.zero;
            scrollRT.offsetMin = new Vector2(-20, -17);

            GameObject viewport = new GameObject("Viewport");
            Image viewportImage = viewport.AddComponent<Image>();

            RectTransform viewportRT = viewport.GetComponent<RectTransform>();
            viewportRT.SetParent(rowsRt);

            scrollRect.viewport = viewportRT;

            viewportRT.localScale = Vector3.one;
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.anchoredPosition3D = Vector3.zero;
            viewportRT.offsetMin = Vector2.zero;
            viewportRT.offsetMax = Vector2.zero;

            viewport.AddComponent<Mask>();

            GameObject content = new GameObject("Content");
            content.transform.parent = viewport.transform;

            RectTransform contentRT = content.AddComponent<RectTransform>();
            scrollRect.content = contentRT;
            contentRT.pivot = new Vector2(0.5f, 1);
            contentRT.localScale = Vector3.one;
            contentRT.anchorMin = new Vector2(0f, 1f);
            contentRT.anchorMax = Vector2.one;
            contentRT.anchoredPosition3D = Vector3.zero;
            contentRT.offsetMin = Vector2.zero;
            contentRT.offsetMax = Vector2.zero;

            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 1;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;

            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRT.SetSiblingIndex(1);

            DataGridView dgv = dataGridView.GetComponent<DataGridView>();
            dgv.rows.changed.AddListener(dgv.OnChange);
            dgv.columnsComponent = header;
            dgv.rowsComponent = content;
        }
#endif
    }
}