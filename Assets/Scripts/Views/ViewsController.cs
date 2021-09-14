using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewsController : MonoBehaviour
{
    public RectTransform rootRT;
    public GameObject template;
    public GameObject target;

    List<List<CameraView>> views = new List<List<CameraView>>()
    {
        new List<CameraView>()
    };

    public List<ViewPreset> presets = new List<ViewPreset>()
    {
        new ViewPreset("1x1", new List<Vector2>() { new Vector2(0, 0)}),
        new ViewPreset("1x2", new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) }, 15),
        new ViewPreset("3x1", new List<Vector2>() { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2) }, 20)
    };

    public UnityEvent viewChanged = new UnityEvent();
    public Vector2 padding = new Vector2(0, 100);
    public Vector2 offset;
    public int currentPresetId = 0;

    private Vector2 boxPadding;

    void Start()
    {
        viewChanged.AddListener(RecalculateViews);
    }

    public void ApplyPreset(string presetName)
    {
        ViewPreset viewPreset = presets.Find(p => p.name == presetName);

        if (viewPreset != null)
        {
            RemoveAllViews();
            viewPreset.views.ForEach(v => AddView(v));
        }
        else
        {
            throw new Exception("Пресета с таким именем не существует");
        }
    }

    public void AddView(Vector2 position)
    {
        if (views.Count <= position.x)
        {
            views.Add(new List<CameraView>());
        }

        GameObject instance = Instantiate(template, transform);
        CameraView view = instance.GetComponent<CameraView>();
        view.cameraRotate.target = target.transform;

        views[(int)position.x].Insert((int)position.y, view);

        viewChanged.Invoke();
    }

    public void RemoveView(Vector2 position)
    {
        int i = (int)position.x;
        int j = (int)position.y;

        CameraView view = views[i][j];

        if (view != null)
        {
            views[i].Remove(view);
            Destroy(view.gameObject);

            if (views[i].Count == 0)
            {
                views.RemoveAt(i);
            }

            viewChanged.Invoke();
        }
    }

    public void RemoveAllViews()
    {
        for (int i = views.Count - 1; i >= 0; i--)
        {
            List<CameraView> row = views[i];
            for (int j = row.Count - 1; j >= 0; j--)
            {
                CameraView view = row[j];
                Destroy(view.gameObject);
                row.RemoveAt(j);
            }

            views.RemoveAt(i);
        }

        viewChanged.Invoke();
    }

    private IEnumerator _RecalculateViews()
    {
        yield return new WaitForEndOfFrame();

        if (rootRT == null)
        {
            throw new Exception("Необходим компонент rootRT");
        }

        boxPadding = new Vector2(Mathf.Abs(rootRT.sizeDelta.x), Mathf.Abs(rootRT.sizeDelta.y));
        boxPadding += padding;

        float height = ((Screen.height - boxPadding.y) / Screen.height) / views.Count;
        float heightRatio = 1f / views.Count;

        for (int i = 0, y = views.Count - 1; i < views.Count; i++, y--)
        {
            float width = ((Screen.width - boxPadding.x) / Screen.width) / views[i].Count;
            float widthRatio = 1f / views[i].Count;

            for (int j = 0; j < views[i].Count; j++)
            {
                CameraView view = views[i][j];
                view.boxCollider.size = new Vector3(Screen.width * widthRatio, Screen.height * heightRatio, 0);
                Rect rect = new Rect();

                rect.width = width;
                rect.height = height;

                float paddingX = 1 - (Screen.width - offset.x) / Screen.width;
                float paddingY = 1 - (Screen.height - offset.y) / Screen.height;

                rect.x = width * j + (boxPadding.x / Screen.width) / 2 + paddingX;
                rect.y = height * y + (boxPadding.y / Screen.height) / 2 - paddingY;

                view.camera.rect = rect;
                view.camera.orthographicSize = presets[currentPresetId].size;
            }
        }
        
    }

    public void RecalculateViews()
    {
        StartCoroutine(_RecalculateViews());
    }
}