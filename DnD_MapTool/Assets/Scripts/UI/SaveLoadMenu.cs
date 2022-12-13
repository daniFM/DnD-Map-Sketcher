// Copyright (c) Daniel Fernández 2022


using UnityEngine;
using MyUtilities;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject browserPanel;

    public void Save()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            string data = JsonUtility.ToJson(TileController.instance.GetLastSnapshot(), true);
            Explorer.instance.Save(data, JSONSaver.extension);
        #else
            Explorer.PathLoaded += OnPathLoadedSave;
            Explorer.instance.GetPath(NetworkManager.instance.GetCurrentRoomName(), JSONSaver.extension, ExplorerMode.Save);
        #endif
    }

    private void OnPathLoadedSave(Path path)
    {
        Explorer.PathLoaded -= OnPathLoadedSave;

        JSONSaver.Save(TileController.instance.GetLastSnapshot(), path.folder, path.name);
    }

    public void Load()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            Explorer.DataLoaded += OnDataLoaded;
            Explorer.instance.Load(JSONSaver.extension);
        #else
            Explorer.PathLoaded += OnPathLoadedLoad;
            Explorer.instance.GetPath(JSONSaver.name, JSONSaver.extension, ExplorerMode.Load);
        #endif
    }

    private void OnPathLoadedLoad(Path path)
    {
        Explorer.PathLoaded -= OnPathLoadedSave;

        TileController.instance.LoadSnapshot(JSONSaver.Load<TileData>(path.folder, path.name), false); // works with false
    }

    private void OnDataLoaded(string data)
    {
        Explorer.DataLoaded -= OnDataLoaded;

        TileController.instance.LoadSnapshot(JsonUtility.FromJson<TileData>(data), false); // works with false
    }

    //private void OpenExplorerSave()
    //{
    //    string path;
    //    string name;
    //    path = StandaloneFileBrowser.SaveFilePanel(
    //        "Save Map",
    //        JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
    //        JSONSaver.name,
    //        JSONSaver.extension);

    //    if(path != string.Empty)
    //    {
    //        int n = path.LastIndexOf('\\');
    //        name = path.Substring(n + 1, path.LastIndexOf('.') - n - 1);
    //        path = path.Substring(0, n);

    //        //Debug.Log("Saving map to: " + pathName[0] + ", " + pathName[1]);
    //        JSONSaver.Save(TileController.instance.GetLastSnapshot(), path, name);
    //    }
    //    else
    //    {
    //        Debug.Log("Player cancelled explorer");
    //    }

    //    #if UNITY_EDITOR

    //    #else
    //        explorerThreadSave.Abort();
    //    #endif
    //}

    //private void OpenExplorerLoad()
    //{
    //    string[] pathArr;
    //    string path;
    //    string name;
    //    pathArr = StandaloneFileBrowser.OpenFilePanel(
    //        "Load Map",
    //        JSONSaver.defaultPath + "/" + JSONSaver.defaultFolder,
    //        JSONSaver.extension,
    //        false);

    //    if(pathArr != null)
    //    {
    //        path = pathArr[0];

    //        int n = path.LastIndexOf('\\');
    //        name = path.Substring(n + 1, path.LastIndexOf('.') - n - 1);
    //        path = path.Substring(0, n);

    //        //Debug.Log("Loading map from: " + pathName[0] + ", " + pathName[1]);
    //        TileController.instance.LoadSnapshot(JSONSaver.Load<TileData>(path, name), false); // works with false
    //    }
    //    else
    //    {
    //        Debug.Log("Player cancelled explorer");
    //    }

    //    #if UNITY_EDITOR

    //    #else
    //        explorerThreadLoad.Abort();
    //    #endif
    //}
    //#endif
}
