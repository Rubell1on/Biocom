using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [Range(0.1f, 50f)] public float speed = 15f;
    [Range(0.1f, 1f)] public float scale = 0.97f;
    private CanvasGroup canvasGroup;
    private Coroutine move;
    private ModalWindow modalWindow;

    void Start()
    {
        modalWindow = GetComponent<ModalWindow>();
        modalWindow.modalWindowClosing.AddListener(OnWindowsClosing);

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(SetTransition(() => move = null));
    }

    private IEnumerator SetTransition(UnityAction callback, bool appear = true)
    {
        float lerp = appear == true ? 0 : 1;
        Vector3 targetScale = transform.localScale;
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

    public void OnWindowsClosing(ModalWindowCloseEventArgs eventArgs)
    {
        eventArgs.cancel = true;
        StartCoroutine(SetTransition(() => 
        {
            modalWindow.modalWindowClosing.RemoveListener(OnWindowsClosing);
            modalWindow.Close();
        }, false));
        
    }
}
