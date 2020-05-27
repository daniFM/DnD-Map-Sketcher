using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Token : MonoBehaviourPun
{
    //public Player controlledBy;
    public bool selected;
    public LayerMask tokenLayer;
    public LayerMask movementLayers;
    public Material highlightedMaterial;

    private bool active;
    private new Renderer renderer;
    private Material mainMaterial;
    private Rigidbody rb;
    private Photon.Realtime.Player player;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        mainMaterial = renderer.material;
        rb = GetComponent<Rigidbody>();
        player = photonView.Owner;

        ToolChanged();
    }

    void OnEnable()
    {
        GameController.OnToolChanged += ToolChanged;
    }

    void OnDisable()
    {
        GameController.OnToolChanged -= ToolChanged;
    }

    void Update()
    {
        if(active)
        {
            if(Input.GetMouseButtonDown(0) && (photonView.IsMine || GameManager.instance.isDM)) // --> IsMine is preventing correct assignation
            {
                //Debug.Log("Checking from " + gameObject.name);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, tokenLayer) && hit.transform == transform)
                {
                    //if(!photonView.IsMine)
                    if(!photonView.IsMine && (GameManager.instance.isDM || player == photonView.Owner))
                        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

                    selected = true;
                    renderer.material = highlightedMaterial;
                }
                else if(selected)
                {
                    renderer.material = mainMaterial;
                    selected = false;
                }
                rb.isKinematic = true;
            }

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
                    transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, Mathf.Floor(transform.position.z));
                    if(!TileController.instance.snapToCenter)
                        transform.Translate(0.5f, 0, 0.5f);

                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = false;
                }
            }

            if(!rb.isKinematic && rb.IsSleeping())
            {
                rb.isKinematic = true;
                //if(player != photonView.Owner)
                //    photonView.TransferOwnership(player);
            }
        }
    }

    private void ToolChanged()
    {
        if(GameController.instance.Tool == ToolType.selection)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }
}
