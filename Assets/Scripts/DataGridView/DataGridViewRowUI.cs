using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatchyClick
{
    [Serializable]
    public class DataGridViewRowUI : MonoBehaviour
    {
        public List<DataGridViewCellUI> cells = new List<DataGridViewCellUI>();

        public static GameObject CreateRow()
        {
            GameObject row = new GameObject("Row");
            RectTransform rowRT = row.AddComponent<RectTransform>();
            rowRT.sizeDelta = new Vector2(100f, 50f);
            DataGridViewRowUI rowUI = row.AddComponent<DataGridViewRowUI>();
            HorizontalLayoutGroup horLayout = row.AddComponent<HorizontalLayoutGroup>();
            horLayout.spacing = 1;
            horLayout.childForceExpandWidth = false;
            horLayout.childControlHeight = false;
            horLayout.childControlWidth = false;
            horLayout.childScaleHeight = true;
            horLayout.childScaleWidth = true;

            return row;
        }
    }
}
