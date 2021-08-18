using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagesLoader : MonoBehaviour
{
    public string path = "C://tmp/images";
    public List<Texture2D> textures;
    public ImageViewer viewer;
    public Vector2 imageSize = new Vector2(512, 412);

    void Start()
    {
        List<string> directories = Directory.GetDirectories(path).ToList();

        if (directories.Count > 0)
        {
            List<string> filePaths = Directory.GetFiles(directories[0]).ToList();

            StartCoroutine(LoadImages(
                filePaths, 
                (textures) => viewer.SetImages(textures)));
        }
    }

    private IEnumerator LoadImages(List<string> paths, Action<List<Texture2D>> loadedCallback)
    {
        List<Texture2D> textures = new List<Texture2D>();

        foreach(string path in paths)
        {
            WWW www = new WWW(path);
            yield return www;
            Texture2D texTmp = new Texture2D((int)imageSize.x, (int)imageSize.y, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texTmp);
            textures.Add(texTmp);
        }

        loadedCallback(textures);
    }
}
