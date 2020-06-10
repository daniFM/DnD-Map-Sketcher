using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Token : MonoBehaviourPun
{
    public LayerMask movementLayers;
    public float sleepTime = 1;
    public bool destroyOnPlayerLeave = true;

    private bool selected;
    private new Renderer renderer;
    private Material mainMaterial;
    private Rigidbody rb;
    private WaitForSeconds waitSleep;
    private Photon.Realtime.Player player;
    private bool initialized;

    void Start()
    {
        if(!initialized)
        {
            Init(GameController.instance.GetPlayerColor(photonView.Owner.ActorNumber - 1));
        }
    }

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

    public void Init(Color color)
    {
        renderer = GetComponent<Renderer>();
        mainMaterial = renderer.material;
        rb = GetComponent<Rigidbody>();
        waitSleep = new WaitForSeconds(sleepTime);
        player = photonView.Owner;

        mainMaterial.SetColor("_BaseColor", color);

        if(!photonView.IsMine)
        {
            SetPhysicsActive(false);
        }

        initialized = true;
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

                //StartCoroutine(FakeOnSleep());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(photonView.IsMine && rb.IsSleeping() && other.gameObject.layer != gameObject.layer)
        {
            //Reposition(1.4f);
            photonView.RPC("Reposition", RpcTarget.All, 1.4f);
        }
    }

    public void SetPhysicsActive(bool activate)
    {
        rb.isKinematic = !activate;
        //rb.detectCollisions = activate;
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
        transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, Mathf.Floor(transform.position.z));
        if(!TileController.instance.snapToCenter)
            transform.Translate(0.5f, height, 0.5f);
    }

    //private IEnumerator FakeOnSleep()
    //{
    //    yield return waitSleep;

    //    rb.isKinematic = true;
    //    //Debug.Log("Kinematic true");
    //}

    private void CheckLeavingPlayer(int id, string name)
    {
        if(photonView.Owner == null)
        {
            if(destroyOnPlayerLeave)
                PhotonNetwork.Destroy(gameObject);
            else
                photonView.TransferOwnership(PhotonNetwork.MasterClient);   //not tested
        }
    }
}
