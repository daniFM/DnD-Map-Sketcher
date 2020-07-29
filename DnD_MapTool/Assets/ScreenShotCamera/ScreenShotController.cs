using System.Collections;
using UnityEngine;
using MyUtilities;

public class ScreenShotController: MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    public GameObject[] deactivateBeforeScrenshot;
    private bool[] prevObjectState;

    private const string defaultName = "map";
    private const string extension = "png";

    //public KeyCode key;

    //private bool takeShot = false;
    private string path;

    //void LateUpdate()
    //{
    //    takeShot |= Input.GetKeyDown(key);

    //    if(takeShot && !string.IsNullOrEmpty(path))
    //    {
    //        Camera camera = GetComponent<Camera>();
    //        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //        camera.targetTexture = rt;
    //        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //        camera.Render();
    //        RenderTexture.active = rt;
    //        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //        camera.targetTexture = null;
    //        RenderTexture.active = null; // JC: added to avoid errors
    //        Destroy(rt);
    //        byte[] bytes = screenShot.EncodeToPNG();
    //        string filename = NameScreenShot(resWidth, resHeight);
    //        System.IO.File.WriteAllBytes(filename, bytes);
    //        Debug.Log("Took screenshot to: " + filename);
    //        takeShot = false;
    //    }
    //}

    public void TakeScreenshot()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            StartCoroutine(TakeScreenshotRoutine("DOWNLOAD"));
        #else
            Explorer.PathLoaded += OnPathLoaded;
            Explorer.instance.GetPath(defaultName, extension, ExplorerMode.Save);
        #endif
    }

    private void OnPathLoaded(Path path)
    {
        Explorer.PathLoaded -= OnPathLoaded;
        StartCoroutine(TakeScreenshotRoutine(path.ToString()));
    }

    private IEnumerator TakeScreenshotRoutine(string path)
    {
        prevObjectState = new bool[deactivateBeforeScrenshot.Length];

        for(int i = 0; i < deactivateBeforeScrenshot.Length; ++i)
        {
            prevObjectState[i] = deactivateBeforeScrenshot[i].activeSelf;
            deactivateBeforeScrenshot[i].SetActive(false);
        }

        yield return null;
        yield return new WaitForEndOfFrame();

        //Camera camera = GetComponent<Camera>();
        //RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        //camera.targetTexture = rt;
        //Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        //camera.Render();
        //RenderTexture.active = rt;
        //screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        //camera.targetTexture = null;
        //RenderTexture.active = null; // JC: added to avoid errors
        //Destroy(rt);
        //byte[] bytes = screenShot.EncodeToPNG();
        //System.IO.File.WriteAllBytes(path, bytes);

        #if UNITY_WEBGL && !UNITY_EDITOR
            Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
            byte[] bytes = screenShot.EncodeToPNG();
            Explorer.instance.Save(bytes, extension);
        #else
            ScreenCapture.CaptureScreenshot(path);
        #endif

        Debug.Log("Took screenshot to: " + path);

        yield return null;

        for(int i = 0; i < deactivateBeforeScrenshot.Length; ++i)
        {
            if(prevObjectState[i])
                deactivateBeforeScrenshot[i].SetActive(true);
        }
    }

    //public static string NameScreenShot(int width, int height)
    //{
    //    string folder_path = Application.dataPath + "/screenshots";

    //    if(!System.IO.Directory.Exists(folder_path))
    //        System.IO.Directory.CreateDirectory(folder_path);

    //    return string.Format("{0}/screen_{1}x{2}_{3}.png",
    //                         folder_path,
    //                         width, height,
    //                         System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    //}
}
