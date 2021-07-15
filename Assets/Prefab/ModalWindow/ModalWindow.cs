using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalWindow : MonoBehaviour
{
    public Text title;
    public Button close;
    public GameObject body;

    public ModalWindowCloseEvent modalWindowClosing = new ModalWindowCloseEvent();
    public UnityEvent modalWindowClosed = new UnityEvent();

    void Start()
    {
        close.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        close.onClick.RemoveAllListeners();
    }

    public void Close()
    {
        ModalWindowCloseEventArgs eventArgs = new ModalWindowCloseEventArgs();
        modalWindowClosing.Invoke(eventArgs);

        if (!eventArgs.cancel)
        {
            modalWindowClosed.Invoke();
            Destroy(gameObject);
        }
    }
}

public class ModalWindowCloseEventArgs
{
    public bool cancel = false;
}

[Serializable]
public class ModalWindowCloseEvent : UnityEvent<ModalWindowCloseEventArgs> {}