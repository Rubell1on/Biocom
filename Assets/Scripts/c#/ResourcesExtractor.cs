using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ResourcesExtractor
{
    public static bool ExtractTextAsset(string resourcesFileName, string fileExtention = "txt")
    {
        TextAsset resource = Resources.Load<TextAsset>(resourcesFileName);

        if (resource == null)
        {
            return false;
        }

        List<string> filePath = resourcesFileName.Split('/').ToList();
        string fileName = filePath[filePath.Count - 1];
        
        if (filePath.Count > 1)
        {
            filePath = filePath.ToList().GetRange(0, filePath.Count - 1);
        } 
        
        string path = String.Join("/" , filePath);

        if (!CreateDirectorySafe(path)) {
            return false;
        }

        string filePathWithExtention = $"{Application.dataPath}/{path}/{fileName}.{fileExtention}";

        if (File.Exists(filePathWithExtention)) File.Delete(filePathWithExtention);

        FileStream fs = File.Open(filePathWithExtention, FileMode.OpenOrCreate);
        fs.Write(resource.bytes, 0, resource.bytes.Length);
        fs.Close();

        return true;
    }

    static bool CreateDirectorySafe(string path)
    {
        try
        {
            path.Split('/').Aggregate(Application.dataPath, (acc, p) =>
            {
                acc += $"/{p}";
                if (!Directory.Exists(acc)) Directory.CreateDirectory(acc);

                return acc;
            });
        }
        catch(Exception e)
        {
            return false;
        }

        return true;
        
    }
}