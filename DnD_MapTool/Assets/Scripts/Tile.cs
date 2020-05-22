﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;

#pragma warning disable 0649
    [SerializeField]
    private MeshFilter mFilter;
    [SerializeField]
    private new Renderer renderer;
    [SerializeField]
    private new BoxCollider collider;
#pragma warning restore 0649

    private TilePlacing placing;
    private float[] rotations = { 0, 90, 180, 270 };

    public void SetTile(TileType type)
    {
        this.type = type;

        //Check neighbors
        //...
        placing = TilePlacing.center;
        transform.Rotate(0, rotations[Random.Range(0, 4)], 0); // Only for center tiles

        mFilter.mesh = TileController.instance.GetTileMesh(type, placing);

        Vector3 size = renderer.bounds.size;
        collider.center = new Vector3(0, size.y/2, 0);
        collider.size = size;
    }
}
