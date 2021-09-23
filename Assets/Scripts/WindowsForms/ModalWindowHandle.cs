using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowHandle : MonoBehaviour
{
    public RectTransform window;
    private Vector2 mousePos = Vector2.zero;

    private RectTransform rt;
    private BoxCollider2D collider;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();

        SetCollider(rt.rect.size, new Vector2(0f, -rt.rect.height / 2));
    }

    private void SetCollider(Vector2 size, Vector2 offset)
    {
        collider.size = size;
        collider.offset = offset;
    }

    private void OnMouseDown()
    {
        mousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector2 delta = (Vector2)Input.mousePosition - mousePos;
        window.anchoredPosition += delta;

        mousePos = Input.mousePosition;
    }
}
