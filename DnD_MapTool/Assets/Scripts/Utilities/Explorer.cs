using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using SFB;

public class Explorer : MonoBehaviour
{
    public GameObject browserPanel;

    private string name;
    private string extension;

    public static Action<string> PathLoaded;
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
        string name = JSONSaver.name;
        if(string.IsNullOrEmpty(name))
            name = "untitled";

        browserPanel.SetActive(true);
        DownloadFile(gameObject.name, "OnFileDownload", name + "." + extension, bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload()
    {
        Debug.Log("File Successfully Downloaded");
    }

    public void LoadData(string extension)
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
    public void GetPath(string name, string extension)
    {
        this.name = name;
        this.extension = extension;

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
        string path;
        path = StandaloneFileBrowser.SaveFilePanel(
            "Save Map",
            JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
            name,
            extension);

        if(path != string.Empty)
        {
            int n = path.LastIndexOf('\\');
            //name = path.Substring(n + 1, path.LastIndexOf('.') - n - 1);
            string folder = path.Substring(0, n);

            if(!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
        }
        else
        {
            Debug.Log("Player cancelled explorer");
        }

        PathLoaded?.Invoke(path);

        #if UNITY_EDITOR

        #else
            explorerThreadSave.Abort();
        #endif
    }

#endif
}
