using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//public class TileInitData
//{
//    public int Type { get; private set; }

//    public TileInitData(int type)
//    {
//        this.Type = type;
//    }
//}

public class Tile : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public TileType type;

    [SerializeField] private MeshFilter mFilter;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private GameObject subTilePrefab;

    private TilePlacing placing;
    private float[] rotations = { 0, 90, 180, 270 };

    public void SetTile(TileType type)
    {
        this.type = type;

        //Check neighbors
        //...
        placing = TilePlacing.center;

        // Stairs logic
        if(type == TileType.stair1 || type == TileType.stair2 || type == TileType.stair3 || type == TileType.stair4)
        {
            transform.Rotate(0, rotations[type-TileType.stair1], 0);
            type = TileType.stair1;
        }
        //else if(type == TileType.wall)
        //{
        //    for(int i = 0; i < 3; ++i)
        //    {
        //        //Instantiate(subTilePrefab, new Vector3(0, i, 0), Quaternion.Euler(0, rotations[Random.Range(0, 4)], 0), transform);
        //        Transform t = PhotonNetwork.Instantiate(subTilePrefab.name, transform.position, Quaternion.identity).transform;
        //        t.parent = transform;
        //        t.Translate(0, i, 0);
        //        t.Rotate(0, rotations[Random.Range(0, 4)], 0);
        //    }
        //}
        // Other tiles logic
        else
        {
            transform.Rotate(0, rotations[Random.Range(0, 4)], 0); // Only for center tiles
        }

        

        mFilter.mesh = TileController.instance.GetTileMesh(type, placing);

        Vector3 size = renderer.bounds.size;
        collider.center = new Vector3(0, size.y/2, 0);
        collider.size = size;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //info.sender.TagObject = this.GameObject;
        //Debug.Log("OnPhotonInstantiate");
        SetTile((TileType)info.photonView.InstantiationData[0]);
    }

    public void DestroyByAnybody()
    {
        if(!photonView.IsMine)
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        PhotonNetwork.Destroy(gameObject);
    }
}
