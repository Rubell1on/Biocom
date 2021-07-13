using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace DataGridView
{
    public class DataGridViewUI
    {
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

            GameObject header = new GameObject("Header");
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

            GameObject rows = new GameObject("Rows");
            rows.transform.parent = dataGridView.transform;
            rows.AddComponent<Image>();

            RectTransform rowsRt = rows.GetComponent<RectTransform>();
            rowsRt.anchorMin = Vector2.zero;
            rowsRt.anchorMax = Vector2.one;
            rowsRt.pivot = new Vector2(0.5f, 1);
            rowsRt.offsetMin = new Vector2(-15, 0);
            rowsRt.offsetMax = new Vector2(0f, -50f);

            ScrollRect scrollRect = rows.AddComponent<ScrollRect>();
            scrollRect.scrollSensitivity = 20;
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
            slidingAreaRT.offsetMin = new Vector2(10, 0);
            slidingAreaRT.offsetMax = new Vector2(-10, 0);

            GameObject handleObject = new GameObject("Handle");
            Image handleImage = handleObject.AddComponent<Image>();
            handleImage.color = new Color32(180, 180, 180, 255);
            RectTransform handleRT = handleObject.GetComponent<RectTransform>();
            scrollbar.handleRect = handleRT;
            handleRT.SetParent(slidingAreaRT);
            handleRT.anchorMin = new Vector2(0, 1);
            handleRT.anchorMax = Vector2.one;
            handleRT.offsetMin = new Vector2(-10, 0);
            handleRT.offsetMax = new Vector2(10, 0);

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
    }
}