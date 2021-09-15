using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImagesLoader : MonoBehaviour
{
    public string path = "C://tmp/images";
    public List<ImageViewer> viewers;
    public Vector2 imageSize = new Vector2(512, 412);

    //async void Start()
    //{
    //    List<string> directories = Directory.GetDirectories(path).ToList();

    //    if (directories.Count > 0)
    //    {
    //        Texture2D[][] textures = await GetImagesFromDirectories(directories);

    //        for (int i = 0; i < directories.Count; i++)
    //        {
    //            viewers[i].SetImages(textures[i].ToList());
    //        }
    //    }
    //}

    public async Task<Texture2D[][]> GetImagesFromDirectories(List<string> directories)
    {
        Task<Texture2D[]>[] textureTasks = directories.Select(GetImagesTask).ToArray();

        async Task<Texture2D[]> GetImagesTask(string directory)
        {
            List<string> filePaths = Directory.GetFiles(directory).ToList();
            filePaths.Sort(PathsComparer);

            Texture2D[] textures = await LoadImages(filePaths);
            return textures;
        }

        Texture2D[][] textures = await Task.WhenAll(textureTasks);
        return textures;
    }

    public void ClearViews()
    {
        viewers.ForEach(v => v.RemoveImages());
    }

    public async Task<Texture2D[]> LoadImages(List<string> paths)
    {
        Task<Texture2D>[] operations = paths.Select(LoadImage).ToArray();

        Texture2D[] textures = await Task.WhenAll(operations);

        return textures;
    }

    public async Task<Texture2D> LoadImage(string path)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
        await request.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        return texture;
    }

    private int PathsComparer(string a, string b)
    {
        string[] firstPath = a.Split('\\');
        string[] secondPath = b.Split('\\');

        string firstName = firstPath[firstPath.Length - 1].Split('.')[0];
        string secondName = secondPath[secondPath.Length - 1].Split('.')[0];

        int firstId = 0;
        int secondId = 0;

        if (Int32.TryParse(firstName, out firstId) && Int32.TryParse(secondName, out secondId))
        {
            return firstId.CompareTo(secondId);
        }

        return firstName.CompareTo(secondName);
    }
}
