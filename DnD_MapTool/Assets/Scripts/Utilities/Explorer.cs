// Copyright (c) Daniel Fernández 2022


using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using SFB;
using MyUtilities;

namespace MyUtilities
{
    public class Path
    {
        public string folder { get; }
        public string name { get; }
        public string extension { get; }
        public string path { get { return folder + "\\" + name + "." + extension; } set { } }

        public Path(string path)
        {
            this.path = path;
            int b = path.LastIndexOf('\\');
            int p = path.LastIndexOf('.');
            folder = path.Substring(0, b);
            name = path.Substring(b + 1, p - b - 1);
            extension = path.Substring(p);
        }

        public override string ToString()
        {
            return path;
        }
    }
}

public enum ExplorerMode { Save, Load }

public class Explorer : MonoBehaviour
{
    public GameObject browserPanel;

    private string name;
    private string extension;

    private ExplorerMode mode;

    public static Action<Path> PathLoaded;
    public static Action<string> DataLoaded;

    public static Explorer instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void Save(string data, string extension)
    {
        browserPanel.SetActive(true);

        byte[] bytes = Encoding.UTF8.GetBytes(data);
        Save(bytes, extension);
    }

    // Brwoser plugin should be called in OnPointerDown. (this needs IPointerDownHandler implementation)
    public void Save(byte[] bytes, string extension)
    {
        name = NetworkManager.instance.GetCurrentRoomName();
        browserPanel.SetActive(true);
        DownloadFile(gameObject.name, "OnFileDownload", name + "." + extension, bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload()
    {
        Debug.Log("File Successfully Downloaded");
    }

    public void Load(string extension)
    {
        browserPanel.SetActive(true);

        UploadFile(gameObject.name, "OnFileUpload", "." + extension, false);
    }

    // Called from browser
    public void OnFileUpload(string url)
    {
        StartCoroutine(OutputRoutine(url));
    }

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;
        DataLoaded?.Invoke(loader.text);
    }

#else

    private Thread explorerThreadSave;
    private Thread explorerThreadLoad;

    /// <summary>
    /// Result is provided in PathLoaded event. You should subscribe before calling this.
    /// </summary>
    public void GetPath(string name, string extension, ExplorerMode mode)
    {
        this.name = name;
        this.extension = extension;
        this.mode = mode;

        #if UNITY_EDITOR
            GetExplorerPathAsync();
        #else
            //new Thread(GetExplorerPathAsync).Start();
            //explorerThreadSave.Start();
            
            explorerThreadSave = null;
            explorerThreadSave = new Thread(GetExplorerPathAsync);
            explorerThreadSave.Start();
        #endif
    }

    private void GetExplorerPathAsync()
    {
        string pathstr = string.Empty;

        switch(mode)
        {
            case ExplorerMode.Save:

                pathstr = StandaloneFileBrowser.SaveFilePanel(
                    "Save Map",
                    JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
                    name,
                    extension);

                break;
            case ExplorerMode.Load:

                string[] pathArr = StandaloneFileBrowser.OpenFilePanel(
                    "Load Map",
                    JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
                    extension,
                    false);
                if(pathArr != null)
                    pathstr = pathArr[0];

                break;
        }
        
        if(pathstr != string.Empty)
        {
            Path path = new Path(pathstr);

            if(!System.IO.Directory.Exists(path.folder))
                System.IO.Directory.CreateDirectory(path.folder);

            PathLoaded?.Invoke(path);
        }
        else
        {
            Debug.Log("Player cancelled explorer");
        }

        //#if UNITY_EDITOR

        //#else
        //    explorerThreadSave = null;
        //#endif
    }

#endif
}
