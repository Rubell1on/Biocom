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
    public class DataGridViewHeaderElement : MonoBehaviour
    {
        public Text textComponent;

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
            DataGridViewHeaderElement dgvhe = element.AddComponent<DataGridViewHeaderElement>();

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

            dgvhe.textComponent = text;

            return element;
        }
    }
}
