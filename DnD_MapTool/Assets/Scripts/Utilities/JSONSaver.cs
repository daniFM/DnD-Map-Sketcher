using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONSaver
{
    public static string defaultPath = Application.persistentDataPath;
    public static string defaultFolder = "Saves";
    public static string name;
    public static string extension = "map";

    public static void Save<T>(T obj)
    {
        if(string.IsNullOrEmpty(name))
            return;

        string fullPath = MakeFullPath();

        Write(obj, fullPath);
    }

    public static void Save<T>(T obj, string name)
    {
        JSONSaver.name = name;
        string fullPath = MakeFullPath();

        Write(obj, fullPath);
    }

    public static void Save<T>(T obj, string path, string name)
    {
        JSONSaver.defaultPath = path;
        JSONSaver.name = name;
        string fullPath = MakeFullPath(false);

        Write(obj, fullPath);
    }

    public static T Load<T>(string name)
    {
        return Read<T>(MakeFullPath() + "/" + name + "." + extension);
    }

    public static T Load<T>(string path, string name)
    {
        return Read<T>(path + "/" + name + "." + extension);
    }

    private static string MakeFullPath(bool useFolder = true)
    {
        // MakeFullPath now does't add the name and extension, as Write needs to have the path separated
        if(useFolder)
            return defaultPath + "/" + defaultFolder;// + "/" + name + extension;
        else
            return defaultPath;// + "/" + name + extension;
    }

    private static void Write<T>(T obj, string path)
    {
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string finalPath = path + "/" + name + "." + extension;
        Debug.Log("Writing to: " + finalPath);

        string jsonStr = JsonUtility.ToJson(obj, true);
        File.WriteAllText(finalPath, jsonStr);
    }

    private static T Read<T>(string path)
    {
        Debug.Log("Reafing from: " + path);

        string jsonStr = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonStr);
    }

    //private static string MakeFullPath(string path)
    //{
    //    return path + "/" + name + extension;
    //}
}
