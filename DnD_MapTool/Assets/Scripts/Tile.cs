using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;

    [SerializeField]
    private MeshFilter mFilter;

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
    }
}
