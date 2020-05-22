using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brush
{
    [SerializeField]
    private Texture2D originalShape;
    [HideInInspector]
    public Texture2D shape;
    public int size;
    public Color color;
    public bool eraser;

    public Brush()
    {
        //Update();
    }

    public void Update()
    {
        shape = new Texture2D(originalShape.width, originalShape.height);
        //shape = new Texture2D(size, size);
        Graphics.CopyTexture(originalShape, shape);

        // Apply colors to brush texture
        Color[] cols = shape.GetPixels();
        for(int i = 0; i < size*size; ++i)
        {
            cols[i] *= color;
        }
        shape.SetPixels(cols);

        shape.Apply();
    }
}
