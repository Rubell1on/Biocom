using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoubleClick<T>: UnityEvent<T>
{
    private static Coroutine coroutine;
    private static float clickTimer = 0.5f;

    private MonoBehaviour monoBehaviour = null;
    private UnityEvent<T> secondClick = new UnityEvent<T>();

    public DoubleClick()
    {
        base.AddListener(OnFirstClick);
    }

    private void OnFirstClick(T args)
    {
        if (coroutine == null)
        {
            monoBehaviour = new GameObject("DoubleClickListener").AddComponent<DoubleClickMono>();
            coroutine = monoBehaviour.StartCoroutine(WaitSecondClick());
        }
    }
    private IEnumerator WaitSecondClick()
    {
        base.AddListener(OnSecondClick);
        yield return new WaitForSeconds(clickTimer);
        base.RemoveListener(OnSecondClick);
        coroutine = null;
        GameObject.Destroy(monoBehaviour.gameObject);
    }

    private void OnSecondClick(T args)
    {
        base.RemoveListener(OnSecondClick);
        secondClick.Invoke(args);
        monoBehaviour.StopCoroutine(coroutine);
        coroutine = null;
        GameObject.Destroy(monoBehaviour.gameObject);
    }

    public new void AddListener(UnityAction<T> call)
    {
        secondClick.AddListener(call);
    }

    public new void RemoveListener(UnityAction<T> call)
    {
        secondClick.RemoveListener(call);
    }
}

class DoubleClickMono : MonoBehaviour { }