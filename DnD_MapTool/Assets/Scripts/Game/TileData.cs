// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    [SerializeField] private List<TileType> tileTypes;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private List<Quaternion> rotations;

    public int Count { get { return tileTypes.Count; } }

    public TileData()
    {
        tileTypes = new List<TileType>();
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
    }

    public TileData(TileData copy)
    {
        tileTypes = new List<TileType>(copy.tileTypes);
        positions = new List<Vector3>(copy.positions);
        rotations = new List<Quaternion>(copy.rotations);
    }

    public void Add(TileType type, Vector3 position, Quaternion rotation)
    {
        tileTypes.Add(type);
        positions.Add(position);
        rotations.Add(rotation);
    }

    public void Clear()
    {
        tileTypes.Clear();
        positions.Clear();
        rotations.Clear();
    }

    public TileType GetTypeAt(int index)
    {
        return tileTypes[index];
    }

    public Vector3 GetPositionAt(int index)
    {
        return positions[index];
    }

    public Quaternion GetRotationAt(int index)
    {
        return rotations[index];
    }

    //public KeyValuePair<TileType, Vector3> GetDataAt(int index)
    //{
    //    return new KeyValuePair<TileType, Vector3>(tileTypes[index], positions[index]);
    //}

    public void Pop()
    {
        if(Count > 0)
        {
            int lastPos = Count - 1;
            tileTypes.RemoveAt(lastPos);
            positions.RemoveAt(lastPos);
            rotations.RemoveAt(lastPos);
        }
    }

    public void PopFirst()
    {
        if(Count > 0)
        {
            tileTypes.RemoveAt(0);
            positions.RemoveAt(0);
            rotations.RemoveAt(0);
        }
    }
}