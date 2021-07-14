using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace CatchyClick
{
    [Serializable]
    public class DataGridViewCellUI : MonoBehaviour
    {
        public Text textComponent;
        public Button buttonComponent;
        //[MenuItem("GameObject/UI/Custom/DataGridViewCell")]
        public static GameObject CreateCell()
        {
            GameObject cellObject = new GameObject("Cell");
            Button cellButton = cellObject.AddComponent<Button>();
            RectTransform cellRT = cellObject.AddComponent<RectTransform>();
            cellRT.sizeDelta = new Vector2(100f, 50f);

            DataGridViewCellUI cell = cellObject.AddComponent<DataGridViewCellUI>();
            GameObject textObject = new GameObject("Text");

            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "HeaderElement";

            RectTransform textRT = textObject.GetComponent<RectTransform>();
            textRT.SetParent(cellRT);
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            cell.textComponent = text;
            cell.buttonComponent = cellButton;

            return cellObject;
        }

        private void OnDestroy()
        {
            buttonComponent.onClick.RemoveAllListeners();

        }
    }
}
