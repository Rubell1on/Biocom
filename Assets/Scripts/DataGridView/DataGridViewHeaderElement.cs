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
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            dgvhe.textComponent = text;

            return element;
        }
    }
}
