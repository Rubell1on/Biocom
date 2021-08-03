using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsTransform : MonoBehaviour
{
    public GameObject ui;
    private Canvas canvas;
    public enum Direction { Left, Up, Right, Down };
    public Direction selecteDirection;
    public float pos = 10f;
    public bool instant = false;
    [Range(0.1f, 5)] public float speed = 2;
    List<Vector3> list;
    private Coroutine move;

    void Start()
    {

        list = new List<Vector3>()
        {
            new Vector3(pos * -1, 0),
            new Vector3(0, pos * -1),
            new Vector3(pos, 0),
            new Vector3(0, pos),
        };
    }

    public void ShowCanvas()
    {
        Instantiate(ui);
        canvas = ui.GetComponent<Canvas>();
        if (move == null)
        {
            canvas.gameObject.SetActive(true);
            move = StartCoroutine(Move(() =>
            {
                move = null;
            }));
        }
    }

    public IEnumerator Move(UnityAction callback)
    {
        Vector3 targetPosition = list[(int)selecteDirection];
        float lerp = 0;

        RectTransform transform = canvas.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();

        if (instant == false)
        {
            while (lerp < 1 / speed)
            {
                canvasGroup.alpha = lerp * speed;
                transform.anchoredPosition = Vector3.Lerp(targetPosition, Vector3.zero, lerp * speed);
                lerp += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            canvasGroup.alpha = 1;
            transform.anchoredPosition = Vector3.zero;
        }
        callback();
    }
}
