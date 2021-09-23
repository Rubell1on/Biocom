using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalWindow : MonoBehaviour
{
    public Text title;
    public Image icon;
    public Button close;
    public GameObject body;
    public List<GameObject> alsoDestroy = new List<GameObject>();

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
            alsoDestroy.ForEach(o => Destroy(o));
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