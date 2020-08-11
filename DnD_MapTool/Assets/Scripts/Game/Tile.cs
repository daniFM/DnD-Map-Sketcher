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
    private Renderer rend;
    private Material material;
    public Material animMaterial;

    public void SetTile(TileType type)
    {
        this.type = type;
        placing = TilePlacing.center;
        rend = GetComponent<Renderer>();
        material = rend.sharedMaterial;
        rend.material = animMaterial;
        //rend.material.SetFloat("_InitTime", rend.material.GetFloat("_Time"));
        //Invoke("SetMaterialBack", 2);
        StartCoroutine(AppearRoutine());

        // Tiles with random-rotate
        if(type == TileType.column || type == TileType.groundHigh || type == TileType.groundLow || type == TileType.groundMH || type == TileType.groundML || type == TileType.orb)
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

    public void RotateTile()
    {
        transform.Rotate(Vector3.up, 90);
    }

    //private void SetMaterialBack()
    //{
    //    rend.sharedMaterial = material;
    //}

    private IEnumerator AppearRoutine()
    {
        float size = 0;

        do
        {
            rend.material.SetFloat("_TimeSize", size);
            size += 0.01f;

            yield return null;
        } while(size < 1);

        size = 1;
        rend.material.SetFloat("_TimeSize", size);

        yield return null;

        rend.sharedMaterial = material;
    }
}
