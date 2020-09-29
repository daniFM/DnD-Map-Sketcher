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
    private int rotation;
    private float[] rotations = { 0, 90, 180, 270 };

    public void SetTile(TileType type)
    {
        this.type = type;

        placing = TilePlacing.center;

        // Tiles with random-rotate
        if(type == TileType.column || type == TileType.groundHigh || type == TileType.groundLow || type == TileType.groundMH || type == TileType.groundML || type == TileType.orb)
        {
            rotation = Random.Range(0, 4);
            transform.Rotate(0, rotations[rotation], 0); // Only for center tiles
        }
        // Tiles with auto-rotate
        else if(type == TileType.wall)
        {
            //debug
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.parent = transform;
            wall.transform.localPosition = new Vector3(0.4f, 0.5f, 0);
            wall.transform.localScale = new Vector3(0.1f, 1, 1);
            //debug
            AutoRotate();
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

        rotation++;
        if(rotation >= 4)
            rotation = 0;
    }

    private int GetRotation()
    {
        return rotation;
    }

    private void AutoRotate()
    {
        //Collider[] adjacents = Physics.OverlapSphere(transform.position, 1, 1 << gameObject.layer);
        Collider[] adjacents = Physics.OverlapBox(transform.position + new Vector3(0, 0.5f, 0), new Vector3(0.4f, 0.2f, 0.4f), Quaternion.Euler(0, 45, 0), 1 << gameObject.layer);
        Debug.Log("Found " + (adjacents.Length - 1) + " adjacents: " + adjacents);

        foreach (Collider coll in adjacents)
        {
            Tile tile = coll.GetComponent<Tile>();
            if(coll != collider && tile.type == TileType.wall)
            {
                // Safe check if it's in the same X
                if (Mathf.Abs(coll.transform.position.x - transform.position.x)> 0.1f)
                {
                    if(tile.rotation == rotation)
                    {
                        Debug.Log("Same rotation... rotating?");
                        transform.eulerAngles = new Vector3(0, rotations[rotation], 0);
                    }
                }
                // So this is the same Z
                else
                {
                    if(tile.rotation == rotation)
                    {
                        Debug.Log("Different rotation... rotating?");
                        rotation = tile.rotation;
                        transform.eulerAngles = new Vector3(0, rotations[rotation], 0);
                    }
                }
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawSphere(transform.position, 1);
    //}
}
