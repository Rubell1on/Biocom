using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessManager : MonoBehaviour
{
    public InputField inputField;
    public Button button;

    void Start()
    {
        button.onClick.AddListener(CreateProcess);
    }

    private void CreateProcess()
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo("py", $"\"./processResult.py\" \"{inputField.text}\"");
        startInfo.UseShellExecute = false;

        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        int exitCode = process.ExitCode;
        UnityEngine.Debug.Log($"Exit code is: {exitCode}");
    }
}
