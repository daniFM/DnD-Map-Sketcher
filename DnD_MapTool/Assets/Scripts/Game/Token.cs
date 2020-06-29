using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Token : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public LayerMask movementLayers;
    public float sleepTime = 1;
    public bool destroyOnPlayerLeave = true;

    private bool selected;
    private new Renderer renderer;
    private Material mainMaterial;
    private Rigidbody rb;
    private Photon.Realtime.Player player;
    //private bool initialized;

    //void Start()
    //{
    //    if(!initialized)
    //    {
    //        Init(GameController.instance.GetPlayerColor(photonView.Owner.ActorNumber - 1));
    //    }
    //}

    private void OnEnable()
    {
        NetworkManager.OnPlayerLeft += CheckLeavingPlayer;
    }

    private void OnDisable()
    {
        NetworkManager.OnPlayerLeft -= CheckLeavingPlayer;
    }

    void OnDestroy()
    {
        mainMaterial.SetColor("_EmissionColor", Color.clear);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerIndex = (int)info.photonView.InstantiationData[0];
        Init(GameController.instance.GetPlayerColor(playerIndex));
        if(playerIndex != photonView.OwnerActorNr)
        {
            photonView.TransferOwnership(playerIndex);
        }
    }

    public void Init(Color color)
    {
        renderer = GetComponent<Renderer>();
        mainMaterial = renderer.material;
        rb = GetComponent<Rigidbody>();
        player = photonView.Owner;

        mainMaterial.SetColor("_BaseColor", color);

        SetPhysics();

        //initialized = true;
    }

    void Update()
    {
        if(selected)
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, movementLayers))
                {
                    transform.position = hit.point + Vector3.up * 0.4f;
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                selected = false;

                Reposition(0);

                mainMaterial.SetColor("_EmissionColor", Color.clear);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = false;
                //Debug.Log("Kinematic false");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KillBox")
        {
            GameController.instance.Tooltip("You can spawn another token by clicking over the player portraits with the Selection tool.", 8);
            PhotonNetwork.Destroy(photonView);
        }
        else if(photonView.IsMine && rb.IsSleeping() && other.gameObject.layer != gameObject.layer)
        {
            photonView.RPC("Reposition", RpcTarget.All, Random.Range(1.4f, 1.5f));
        }
    }

    public bool Select()
    {
        selected = false;

        rb.isKinematic = true;
        //Debug.Log("Kinematic true");

        if(photonView.IsMine)
        {
            selected = true;
            mainMaterial.SetColor("_EmissionColor", GameController.instance.highlightColor);
        }
        else if(GameManager.instance.isDM || player == PhotonNetwork.LocalPlayer)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            selected = true;
            mainMaterial.SetColor("_EmissionColor", GameController.instance.highlightColor);
        }

        return selected;
    }

    [PunRPC]
    private void Reposition(float height)
    {
        Vector3 repos = new Vector3(Mathf.Floor(transform.position.x), transform.position.y + height, Mathf.Floor(transform.position.z));
        if(!TileController.instance.snapToCenter)
            repos += new Vector3(0.5f, 0, 0.5f);
        transform.position = repos;
    }

    public void SetPhysics()
    {
        if(!photonView.IsMine)
            rb.isKinematic = true;
    }

    private void CheckLeavingPlayer(int id, string name)
    {
        if(photonView.Owner == null)
        {
            if(destroyOnPlayerLeave)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                photonView.TransferOwnership(PhotonNetwork.MasterClient);   //not tested
            }
        }
    }
}
