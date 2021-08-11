using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewsController : MonoBehaviour
{
    public GameObject template;
    public GameObject target;

    List<List<CameraView>> views = new List<List<CameraView>>()
    {
        new List<CameraView>()
    };

    public UnityEvent viewChanged = new UnityEvent();

    void Start()
    {
        viewChanged.AddListener(RecalculateViews);
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

    private IEnumerator _RecalculateViews()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0, y = views.Count - 1; i < views.Count; i++, y--)
        {
            for (int j = 0; j < views[i].Count; j++)
            {
                float width = 1f / views[i].Count;
                float height = 1f / views.Count;

                CameraView view = views[i][j];
                view.boxCollider.size = new Vector3(Screen.width * width, Screen.height * height, 0);
                Rect rect = new Rect();
                rect.width = width;
                rect.height = height;
                rect.x = width * j;
                rect.y = height * y;

                view.camera.rect = rect;
            }
        }
        
    }

    public void RecalculateViews()
    {
        StartCoroutine(_RecalculateViews());
    }
}
