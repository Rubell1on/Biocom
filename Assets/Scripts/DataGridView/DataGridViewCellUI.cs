using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace DataGridView
{
    [Serializable]
    public class DataGridViewCellUI : MonoBehaviour
    {
        public Text textComponent;
        //[MenuItem("GameObject/UI/Custom/DataGridViewCell")]
        public static GameObject CreateCell()
        {
            GameObject cellObject = new GameObject("Cell");
            cellObject.AddComponent<Button>();
            RectTransform cellRT = cellObject.AddComponent<RectTransform>();
            cellRT.sizeDelta = new Vector2(100f, 50f);

            DataGridViewCellUI cell = cellObject.AddComponent<DataGridViewCellUI>();
            GameObject textObject = new GameObject("Text");
            textObject.transform.parent = cellObject.transform;

            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "HeaderElement";

            cell.textComponent = text;

            return cellObject;
        }
    }
}
