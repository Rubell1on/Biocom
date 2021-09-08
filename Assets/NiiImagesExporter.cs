using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class NiiImagesExporter
{
    private static Process process;
    private static TaskCompletionSource<bool> eventHandle;

    public static async Task<bool> Export(string inputFilePath, string outputDirPath)
    {
        eventHandle = new TaskCompletionSource<bool>();

        using (process = new Process())
        {
            try
            {
                string pathToScript = $"{UnityEngine.Application.dataPath}/Scripts/python/SimpleITK/ExportImages.py";
                ProcessStartInfo startInfo = new ProcessStartInfo("powershell", $"py \"{pathToScript}\" \"{inputFilePath}\" \"{outputDirPath}\"");
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;

                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(myProcess_Exited);
                process.Start();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
                eventHandle.SetResult(false);
            }

            return await eventHandle.Task;
        }
    }

    private static void myProcess_Exited(object sender, System.EventArgs e)
    {
        eventHandle.TrySetResult(true);
    }
}
