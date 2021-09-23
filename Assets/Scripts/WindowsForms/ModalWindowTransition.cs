using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowTransition : Transition
{
    private ModalWindow modalWindow;

    void Start()
    {
        modalWindow = GetComponent<ModalWindow>();
        modalWindow.modalWindowClosing.AddListener(OnWindowsClosing);

        StartCoroutine(SetTransition(() => { }));
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