using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.Threading;

public class SaveLoadMenu : MonoBehaviour
{
    private Thread explorerThread;

    // Start is called before the first frame update
    void Start()
    {
        explorerThread = new Thread(OpenExplorerSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        //// Save file
        //string path;
        //path = StandaloneFileBrowser.SaveFilePanel("Save File", string.Empty, string.Empty, string.Empty);
        //Debug.Log("Saving map to: " + path);
        explorerThread.Start();

        //JSONSaver.Save(TileController.instance.GetLastSnapshot(), "test1");
    }

    public void Load()
    {
        TileController.instance.LoadSnapshot(JSONSaver.Load<TileData>("test1"), false); // works with false
    }

    private void OpenExplorerSave()
    {
        string path;
        path = StandaloneFileBrowser.SaveFilePanel("Save File", string.Empty, string.Empty, string.Empty);
        Debug.Log("Saving map to: " + path);
    }
}
