using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

public class ChartDataInitializerTest : MonoBehaviour
{
    public GraphChart graphChart;
    string categoryName = "Test";
    // Start is called before the first frame update
    void Start()
    {
        graphChart.DataSource.AddPointToCategory(categoryName, 0, 0);
        graphChart.DataSource.AddPointToCategory(categoryName, 1, 1);
        graphChart.DataSource.AddPointToCategory(categoryName, 1, 2);
        graphChart.DataSource.AddPointToCategory(categoryName, 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
