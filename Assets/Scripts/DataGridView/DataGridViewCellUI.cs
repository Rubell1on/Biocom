using System;
using UnityEngine;
using UnityEngine.UI;

namespace CatchyClick
{
    [Serializable]
    public class DataGridViewCellUI : MonoBehaviour
    {
        public Text textComponent;
        public Button buttonComponent;

        public static GameObject CreateCell()
        {
            GameObject cellObject = new GameObject("Cell");
            Button cellButton = cellObject.AddComponent<Button>();
            RectTransform cellRT = cellObject.AddComponent<RectTransform>();
            cellRT.sizeDelta = new Vector2(100f, 50f);
            cellObject.AddComponent<Image>();

            DataGridViewCellUI cell = cellObject.AddComponent<DataGridViewCellUI>();
            GameObject textObject = new GameObject("Text");

            Text text = textObject.AddComponent<Text>();
            text.font = Resources.Load<Font>("Fonts/PT Astra Sans/PT-Astra-Sans_Regular");
            text.color = Color.white;
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
