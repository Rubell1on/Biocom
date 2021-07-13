using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DataGridView
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
            element.AddComponent<Button>();
            DataGridViewHeaderElement dgvhe = element.AddComponent<DataGridViewHeaderElement>();

            GameObject textObject = new GameObject("Text");
            textObject.transform.parent = element.transform;
            Text text = textObject.AddComponent<Text>();
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "HeaderElement";

            RectTransform textRT = textObject.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;

            dgvhe.textComponent = text;

            return element;
        }
    }
}
