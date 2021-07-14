using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace CatchyClick
{
    [Serializable]
    public class DataGridViewHeaderElementUI : MonoBehaviour
    {
        public Text textComponent;
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Custom/DataGridViewHeaderElement")]
        public static GameObject CreateHeaderElement()
        {
            GameObject element = new GameObject("HeaderElement");
            element.AddComponent<Image>();
            RectTransform headerRT = element.GetComponent<RectTransform>();

            Transform parent = Selection.activeTransform;
            if (parent)
            {
                headerRT.SetParent(parent);
            }

            headerRT.localScale = Vector3.one;
            headerRT.anchoredPosition3D = Vector3.zero;

            element.AddComponent<Button>();
            DataGridViewHeaderElementUI dgvhe = element.AddComponent<DataGridViewHeaderElementUI>();

            GameObject textObject = new GameObject("Text");
            Text text = textObject.AddComponent<Text>();
            text.fontStyle = FontStyle.Bold;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "HeaderElement";

            RectTransform textRT = textObject.GetComponent<RectTransform>();
            textRT.SetParent(headerRT);
            textRT.localScale = Vector3.one;
            textRT.anchoredPosition3D = Vector3.zero;
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            GameObject horSizeObject = new GameObject("HorizontalSizeHandle");
            Image horSizeImage = horSizeObject.AddComponent<Image>();
            horSizeImage.color = Color.clear;
            RectTransform horSizeRT = horSizeObject.GetComponent<RectTransform>();
            horSizeRT.SetParent(headerRT);
            horSizeRT.localScale = Vector2.one;
            horSizeRT.anchorMin = new Vector2(1, 0);
            horSizeRT.anchorMax = Vector2.one;
            horSizeRT.offsetMin = Vector2.zero;
            horSizeRT.offsetMax = new Vector2(5, 0);
            horSizeRT.anchoredPosition3D = Vector3.zero;

            BoxCollider2D horSizeCollider = horSizeObject.AddComponent<BoxCollider2D>();
            horSizeCollider.offset = new Vector2(-0.0001375675f, -0.02674484f);
            horSizeCollider.size = new Vector2(5.063251f, 49.75875f);

            HeaderElementSizeScaler scaler = horSizeObject.AddComponent<HeaderElementSizeScaler>();
            scaler.wrapperRT = headerRT;

            dgvhe.textComponent = text;

            return element;
        }
#endif
    }
}
