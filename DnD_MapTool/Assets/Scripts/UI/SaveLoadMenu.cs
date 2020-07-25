using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.Threading;

public class SaveLoadMenu : MonoBehaviour
{
    private Thread explorerThreadSave;
    private Thread explorerThreadLoad;

    void Start()
    {
        explorerThreadSave = new Thread(OpenExplorerSave);
        explorerThreadLoad = new Thread(OpenExplorerLoad);
    }

    public void Save()
    {
        #if UNITY_EDITOR
            OpenExplorerSave();
        #else
            explorerThread.Start();
        #endif
    }

    public void Load()
    {
        #if UNITY_EDITOR
            OpenExplorerLoad();
        #else
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
    }
}
