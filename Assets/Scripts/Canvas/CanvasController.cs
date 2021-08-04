using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public List<Canvas> canvas;
    public enum Direction {Left, Up, Right, Down};
    public Direction selecteDirection;
    public bool instant = false;
    [Range(0.1f, 5)]public float speed = 2;
    List<Vector3> list = new List<Vector3>()
    {
        new Vector3(Screen.width * -1, 0),
        new Vector3(0, Screen.height * -1),
        new Vector3(Screen.width, 0),
        new Vector3(0, Screen.height),
    };
    private Canvas current;
    private Canvas previous;
    private Coroutine move;

    void Start()
    {
        for (int i = 0; i < canvas.Count; i++)
        {
            if (i == 0)
            {
                current = canvas[0];
                current.gameObject.SetActive(true);
            }
            else
            {
                canvas[i].gameObject.SetActive(false);
            }
        }     
    }
    public void SelectCanvas(int id)
    {
        if (move == null)
        {
            previous = current;
            current = canvas[id];
            if (previous != current)
            {
                current.gameObject.SetActive(true);
                move = StartCoroutine(Move(() =>
                {
                    previous.gameObject.SetActive(false);
                    move = null;
                }));
            }
        }
    }

    public IEnumerator Move(UnityAction callback)
    {
        Vector3 targetPosition = list[(int)selecteDirection];
        float lerp = 0;

        RectTransform previousTransform = previous.GetComponent<RectTransform>();
        RectTransform currentTransform = current.GetComponent<RectTransform>();

        CanvasGroup previousGroup = previous.GetComponent<CanvasGroup>();
        CanvasGroup currentGroup = current.GetComponent<CanvasGroup>();

        currentTransform.anchoredPosition = list[(int)selecteDirection];

        if (instant == false)
            while (lerp < 1)
            {
                previousGroup.alpha = 1 - lerp;
                currentGroup.alpha = lerp * speed;
                previousTransform.anchoredPosition = Vector3.Lerp(previousTransform.anchoredPosition, targetPosition, lerp * speed);
                currentTransform.anchoredPosition = Vector3.Lerp(currentTransform.anchoredPosition, Vector3.zero, lerp * speed);
                lerp += Time.deltaTime * speed;

                yield return new WaitForEndOfFrame();
            }
        else
        {
            previousGroup.alpha = 0;
            currentGroup.alpha = 1;
            previousTransform.anchoredPosition = targetPosition;
            currentTransform.anchoredPosition = Vector3.zero;
        }
        callback();
    }
}