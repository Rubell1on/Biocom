using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Server
{
    public bool active = true; 
    private HttpListener listener;
    private ServerParams _serverParams;

    private Dictionary<string, Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>> paths = new Dictionary<string, Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>>()
    {
        { "GET", new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>() },
        { "POST", new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>() },
        { "PUT", new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>() },
        { "DELETE", new Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>>() }
    };

    public Server(ServerParams serverParams)
    {
        this._serverParams = serverParams;
    }

    public async Task Listen()
    {
        listener = new HttpListener();
        listener.Prefixes.Add(_serverParams.url);
        listener.Start();

        Console.WriteLine("Ожидание подключений...");

        while (active)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string methodName = request.HttpMethod;
            if (paths.ContainsKey(methodName))
            {
                Dictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> method = paths[methodName];
                string rawUrl = request.RawUrl;

                if (method.ContainsKey(rawUrl))
                {
                    method[rawUrl](request, response);
                } 
                else
                {
                    response.SetStatusCode(404);
                }
            } 
            else
            {
                response.SetStatusCode(400);
            }
        }

        listener.Stop();
        listener.Close();

        return;
    }

    public void Stop()
    {
        this.active = false;
    }

    public Server Get(string searchPath, Action<HttpListenerRequest, HttpListenerResponse> callback)
    {
        paths["GET"].Add(searchPath, callback);
        return this;
    }
}

public static class HttpListenerResponseExtention
{
    public static HttpListenerResponse SetStatusCode(this HttpListenerResponse res, int code)
    {
        res.StatusCode = code;
        return res;
    }

    public static void WriteJSON(this HttpListenerResponse res, object obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        res.Headers.Add("Content-Type", "application/json");
        res.Write(json);
    }

    public static void Write(this HttpListenerResponse res, string message)
    {
        string responseString = message;
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        res.ContentLength64 = buffer.Length;
        Stream output = res.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
}

public class ServerParams
{
    public string origin = "http://localhost";
    public uint port = 80;
    public string url { get { return $"{origin}:{port}/"; } }
}