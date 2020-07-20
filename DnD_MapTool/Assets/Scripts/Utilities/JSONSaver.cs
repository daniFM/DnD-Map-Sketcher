using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONSaver
{
    private static string defaultPath = Application.persistentDataPath;
    private static string defaultFolder = "Saves";
    private static string name;
    private static string extension = ".map";

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

    public static T Load<T>(string path)
    {
        string jsonStr = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonStr);
    }

    private static string MakeFullPath(bool useFolder = true)
    {
        if(useFolder)
            return defaultPath + "/" + defaultFolder + "/" + name + extension;
        else
            return defaultPath + "/" + name + extension;
    }

    private static void Write<T>(T obj, string path)
    {
        string jsonStr = JsonUtility.ToJson(obj, true);
        File.WriteAllText(path, jsonStr);
    }

    //private static string MakeFullPath(string path)
    //{
    //    return path + "/" + name + extension;
    //}
}
