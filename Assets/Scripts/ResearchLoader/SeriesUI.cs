using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeriesUI : MonoBehaviour
{
    public Text id;
    public Text photosCount;
    public Text date;
    public RawImage image;
    public Button button;

    public void SetInfo(int id, int photosCount, string date)
    {
        this.id.text = id.ToString();
        this.photosCount.text = photosCount.ToString();
        this.date.text = date;
    }

    public void SetImage(Texture texture)
    {
        image.texture = texture;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
