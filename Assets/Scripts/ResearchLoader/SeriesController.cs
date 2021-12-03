using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeriesController : MonoBehaviour
{
    public GameObject content;
    public GameObject template;
    public List<SeriesUI> seriesUIs;
    //public Dictionary<int, List<Part>> series = new Dictionary<int, List<Part>>();
    public List<Series> series = new List<Series>();
    public int current = 0;
    public UnityEvent<int> seriesChanged = new UnityEvent<int>();

    private void Start()
    {
        seriesChanged.AddListener(OnSeriesChanged);
    }

    private void OnDestroy()
    {
        seriesChanged.RemoveListener(OnSeriesChanged);
    }

    //public void AddSeriesRange(Dictionary<int, List<Part>> series)
    //{
    //    if (series.Keys.Count > 0)
    //    {
    //        this.series = series;
    //        List<int> keys = this.series.Keys.ToList();

    //        for (int i = 0; i < series.Count; i++)
    //        {
    //            int key = i;
    //            GameObject instance = Instantiate(template, content.transform);
    //            SeriesUI instanceUI = instance.GetComponent<SeriesUI>();
    //            instanceUI.button.onClick.AddListener(OnSeriesClick);
    //            seriesUIs.Add(instanceUI);

    //            void OnSeriesClick()
    //            {
    //                if (key != current)
    //                {
    //                    seriesChanged.Invoke(key);
    //                }
    //            }
    //        }
    //    }
    //}

    public void AddSeriesRange(List<Series> series)
    {
        this.series = series;

        for (int i = 0; i < series.Count; i++)
        {
            Series s = series[i];
            int key = i;
            GameObject instance = Instantiate(template, content.transform);
            SeriesUI instanceUI = instance.GetComponent<SeriesUI>();
            instanceUI.id.text = s.id.ToString();
            instanceUI.SetImage(s.thumbnail);
            instanceUI.photosCount.text = s.photosCount.ToString();

            instanceUI.button.onClick.AddListener(OnSeriesClick);
            seriesUIs.Add(instanceUI);

            void OnSeriesClick()
            {
                if (key != current)
                {
                    seriesChanged.Invoke(key);
                }
            }
        }
    }

    void OnSeriesChanged(int seriesId)
    {
        current = seriesId;
    }

    public void RemoveSeries()
    {
        seriesUIs.ForEach(s => Destroy(s.gameObject));
        seriesUIs.Clear();
        current = 0;
    }
}
