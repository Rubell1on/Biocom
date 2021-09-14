using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;

public class YesNoWindow : MonoBehaviour
{
    public Text labelText;
    public Button buttonApply;
    public Button buttonCancel;

    public enum DialogResult {NotFinished, Ok, Cancel};
    public DialogResult dialogResult = DialogResult.NotFinished;

    private static TaskCompletionSource<bool> eventHandle;

    public async Task Init(string text = "")
    {
        labelText.text = text;

        buttonApply.onClick.AddListener(() => {
            dialogResult = DialogResult.Ok;
            eventHandle.TrySetResult(true);
        });

        buttonCancel.onClick.AddListener(() => {
            dialogResult = DialogResult.Cancel;
            eventHandle.TrySetResult(false);
        });

        eventHandle = new TaskCompletionSource<bool>();
        await eventHandle.Task;
    }



}

