// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGroup
{
    public TileType type;
    public Mesh[] centers;
    public Mesh[] sides;
    public Mesh[] corners;
}
