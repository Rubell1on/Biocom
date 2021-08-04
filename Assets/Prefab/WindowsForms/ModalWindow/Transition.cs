using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [Range(0.1f, 50f)] public float speed = 15f;
    [Range(0.1f, 1f)] public float scale = 0.97f;

    protected CanvasGroup canvasGroup;
    protected Vector3 initScale;

    public virtual void Awake()
    {
        initScale = transform.localScale;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void SetTransition(bool appear)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        StartCoroutine(SetTransition(() => gameObject.SetActive(appear), appear));
    }

    protected virtual IEnumerator SetTransition(UnityAction callback, bool appear = true)
    {
        float lerp = appear == true ? 0 : 1;
        Vector3 targetScale = initScale;
        Vector3 startScale = targetScale * scale;

        if (appear)
        {
            while (lerp < 1)
            {
                canvasGroup.alpha = lerp;
                transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);
                lerp += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = 1;
        }
        else
        {
            while (lerp > 0)
            {
                canvasGroup.alpha = lerp;
                transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);
                lerp -= Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = 0;
        }
        callback();
    }
}