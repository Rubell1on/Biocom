using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

class Slicer3D
{
    public string exeFileName;
    public string slicerDirPath;
    Process process;

    public Slicer3D(string slicerDirPath, string exeFileName = "Slicer")
    {
        this.exeFileName = exeFileName;
        this.slicerDirPath = slicerDirPath;
    }

    public async Task GenerateMesh(MeshData meshData, ServerParams serverParams)
    {

        Server server = new Server(serverParams);
        server.Get("/event/Slicer3d/connected", OnConnected);
        server.Get("/event/Slicer3d/MeshCreated", OnMeshCreated);

        ProcessStartInfo processInfo = new ProcessStartInfo($"{slicerDirPath}/{exeFileName}", $" --no-main-window --python-script \"{Application.dataPath}/Scripts/python/GenerateMesh.py\" -p {serverParams.port}")
        {
            CreateNoWindow = true,
            UseShellExecute = false
        };

        //CreateProcess($"{Application.dataPath}/Scripts/python/GenerateMesh.py");
        CreateProcess(processInfo);
        await server.Listen();

        return;

        void OnConnected(HttpListenerRequest req, HttpListenerResponse res)
        {
            res.SetStatusCode(200).WriteJSON(meshData);
        }

        void OnMeshCreated(HttpListenerRequest req, HttpListenerResponse res)
        {
            //server.cancellationTokenSource.Cancel();
            res.SetStatusCode(200).Close();
            server.Stop();
            //process.CloseMainWindow();
        }
    }

    void CreateProcess(ProcessStartInfo processInfo)
    {
        process = new Process();
        process.EnableRaisingEvents = false;
        process.StartInfo = processInfo;
        process.Start();
    }
}
