// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

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

    [Header("Animation")]
    public bool doAnimation = true;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Ease easeType;
    [SerializeField] private float overshoot = 1.70158f;

    private TilePlacing placing;
    private float[] rotations = { 0, 90, 180, 270 };

    public void SetTile(TileType type)
    {
        this.type = type;

        placing = TilePlacing.center;

        // Tiles with random-rotate
        if(type == TileType.column || type == TileType.groundHigh || type == TileType.groundLow || type == TileType.groundMH || type == TileType.groundML || type == TileType.orb)
        {
            transform.Rotate(0, rotations[Random.Range(0, 4)], 0); // Only for center tiles
        }

        mFilter.mesh = TileController.instance.GetTileMesh(type, placing);

        Vector3 size = renderer.bounds.size;
        collider.center = new Vector3(0, size.y/2, 0);
        collider.size = size;

        if(doAnimation)
        {
            transform.GetChild(0).localScale = Vector3.zero;
            transform.GetChild(0).DOScale(1, animationTime).SetEase(easeType, overshoot);
        }
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

    public void RotateTile()
    {
        transform.Rotate(Vector3.up, 90);
    }
}
