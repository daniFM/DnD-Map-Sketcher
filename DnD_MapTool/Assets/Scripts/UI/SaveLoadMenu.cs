using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using SFB;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject browserPanel;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    // Broser plugin should be called in OnPointerDown.
    public void Save()
    {
        browserPanel.SetActive(true);

        string data = JsonUtility.ToJson(TileController.instance.GetLastSnapshot(), true);
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        DownloadFile(gameObject.name, "OnFileDownload", "untitled.map", bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload()
    {
        Debug.Log("File Successfully Downloaded");
    }

    public void Load()
    {
        browserPanel.SetActive(true);

        UploadFile(gameObject.name, "OnFileUpload", "." + JSONSaver.extension, false);
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
        TileController.instance.LoadSnapshot(JsonUtility.FromJson<TileData>(loader.text));
    }

#else

    private Thread explorerThreadSave;
    private Thread explorerThreadLoad;

    //void Start()
    //{
    //    explorerThreadSave = new Thread(OpenExplorerSave);
    //    explorerThreadLoad = new Thread(OpenExplorerLoad);
    //}

    public void Save()
    {
        #if UNITY_EDITOR
            OpenExplorerSave();
        #else
            //new Thread(OpenExplorerSave).Start();
            //explorerThreadSave.Start();
            
            explorerThreadSave = null;
            explorerThreadSave = new Thread(OpenExplorerSave);
            explorerThreadSave.Start();
        #endif
    }

    public void Load()
    {
        #if UNITY_EDITOR
            OpenExplorerLoad();
        #else
            //new Thread(OpenExplorerLoad).Start();
            //explorerThreadLoad.Start();
            
            explorerThreadLoad = null;
            explorerThreadLoad = new Thread(OpenExplorerLoad);
            explorerThreadLoad.Start();
        #endif
    }

    private void OpenExplorerSave()
    {
        string path;
        string name;
        path = StandaloneFileBrowser.SaveFilePanel(
            "Save Map",
            JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
            JSONSaver.name,
            JSONSaver.extension);

        if(path != string.Empty)
        {
            int n = path.LastIndexOf('\\');
            name = path.Substring(n + 1, path.LastIndexOf('.') - n - 1);
            path = path.Substring(0, n);

            //Debug.Log("Saving map to: " + pathName[0] + ", " + pathName[1]);
            JSONSaver.Save(TileController.instance.GetLastSnapshot(), path, name);
        }
        else
        {
            Debug.Log("Player cancelled explorer");
        }

        #if UNITY_EDITOR

        #else
            explorerThreadSave.Abort();
        #endif
    }

    private void OpenExplorerLoad()
    {
        string[] pathArr;
        string path;
        string name;
        pathArr = StandaloneFileBrowser.OpenFilePanel(
            "Load Map",
            JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
            JSONSaver.extension,
            false);

        if(pathArr != null)
        {
            path = pathArr[0];

            int n = path.LastIndexOf('\\');
            name = path.Substring(n + 1, path.LastIndexOf('.') - n - 1);
            path = path.Substring(0, n);

            //Debug.Log("Loading map from: " + pathName[0] + ", " + pathName[1]);
            TileController.instance.LoadSnapshot(JSONSaver.Load<TileData>(path, name), false); // works with false
        }
        else
        {
            Debug.Log("Player cancelled explorer");
        }

        #if UNITY_EDITOR

        #else
            explorerThreadLoad.Abort();
        #endif
    }
#endif
}
