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
    public class DataGridViewRowUI : MonoBehaviour
    {
        public List<DataGridViewCellUI> cells = new List<DataGridViewCellUI>();

        public static GameObject CreateRow()
        {
            GameObject row = new GameObject("Row");
            Image image = row.AddComponent<Image>();
            RectTransform rowRT = row.GetComponent<RectTransform>();
            rowRT.sizeDelta = new Vector2(100f, 50f);
            image.color = new Color(.5f, .5f, .5f);
            DataGridViewRowUI rowUI = row.AddComponent<DataGridViewRowUI>();
            HorizontalLayoutGroup horLayout = row.AddComponent<HorizontalLayoutGroup>();
            horLayout.childForceExpandWidth = false;
            horLayout.childControlHeight = false;
            horLayout.childControlWidth = false;
            horLayout.childScaleHeight = true;

            return row;
        }
    }
}
