using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    private int maxSize;
    [SerializeField] private List<TileType> tileTypes;
    [SerializeField] private List<Transform> transforms;

    public TileData(int maxSize)
    {
        this.maxSize = maxSize;
        tileTypes = new List<TileType>();
        transforms = new List<Transform>();
    }

    public int Count { get { return tileTypes.Count; } }
    public int MaxSize { get { return maxSize; } }

    public void Add(TileType type, Transform transform)
    {
        if(Count >= maxSize)
            PopFirst();

        tileTypes.Add(type);
        transforms.Add(transform);
    }

    public void Pop()
    {
        if(Count > 0)
        {
            int lastPos = Count - 1;
            tileTypes.RemoveAt(lastPos);
            transforms.RemoveAt(lastPos);
        }
    }

    public void PopFirst()
    {
        if(Count > 0)
        {
            tileTypes.RemoveAt(0);
            transforms.RemoveAt(0);
        }
    }
}