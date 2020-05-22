using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePainter : MonoBehaviour
{
    public Camera paintCamera;
    public int activeBrush;
    public float mouseSensibility;
    //public Texture2D[] brushes;
    public Brush[] brushes;

    private new Renderer renderer;
    private Texture2D texCopy;
    private int posX;
    private int posY;
    private Brush brush;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        texCopy = Instantiate(renderer.material.mainTexture) as Texture2D;
        renderer.material.mainTexture = texCopy;

        posX = renderer.material.mainTexture.width / 2;
        posY = renderer.material.mainTexture.height / 2;

        brush = brushes[activeBrush];
        brush.Update();
    }

    void Update()
    {
        posX += -(int)(Input.GetAxis("Mouse X") * mouseSensibility);
        posY += -(int)(Input.GetAxis("Mouse Y") * mouseSensibility);
        if (Input.GetMouseButtonDown(0))
        {

            Color[] bcols = brush.shape.GetPixels();
            Color[] tcols = new Color[bcols.Length];// = texCopy.GetPixels();

            int k = 0;
            for(int i = 0; i < brush.size; ++i)
            {
                for(int j = 0; j < brush.size; ++j)
                {
                    // final rgb colour = sourceColour*sourceAlpha + destinationColour*oneMinusSourceAlpha
                    //tcols[i + j] = bcols[i + j] * bcols[i + j].a + texCopy.GetPixel(posX + i, posY + j) * (1 - bcols[i + j].a);

                    Color a = bcols[k];
                    Color b = texCopy.GetPixel(posX + i, posY + j);
                    Color c = a * a.a + b * (1 - a.a);
                    tcols[k] = c;

                    //tcols[i + j] = texCopy.GetPixel(posX + i, posY + j) * bcols[i + j];
                    k++;
                }
            }
            //Color[] tmp = new Color[64*64];
            //for(int i = 0; i < tmp.Length; ++i)
            //{
            //    tmp[i] = Color.yellow;
            //}

            //texCopy.SetPixels32(posX, posY, brushes[activeBrush].size, brushes[activeBrush].size, brush.shape.GetPixels32());
            texCopy.SetPixels(posX, posY, brush.size, brush.size, tcols, 0);
            texCopy.Apply();
        }
    }

    //bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    //{
    //    RaycastHit hit;
    //    Vector3 mousePos = Input.mousePosition;
    //    Vector3 cursorPos = new Vector3(mousePos.x, mousePos.y, 0.0f);
    //    Ray cursorRay = sceneCamera.ScreenPointToRay(cursorPos);
    //    if (Physics.Raycast(cursorRay, out hit, 200))
    //    {
    //        MeshCollider meshCollider = hit.collider as MeshCollider;
    //        if (meshCollider == null || meshCollider.sharedMesh == null)
    //            return false;
    //        Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
    //        uvWorldPosition.x = pixelUV.x;
    //        uvWorldPosition.y = pixelUV.y;
    //        uvWorldPosition.z = 0.0f;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    IEnumerator SaveTextureToFile(Texture2D savedTexture)
    {
        string fullPath = System.IO.Directory.GetCurrentDirectory();
        System.DateTime date = System.DateTime.Now;
        string fileName = "CanvasTexture.png";
        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        yield return null;
    }
}
