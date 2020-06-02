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

                transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, Mathf.Floor(transform.position.z));
                if(!TileController.instance.snapToCenter)
                    transform.Translate(0.5f, 0, 0.5f);

                mainMaterial.SetColor("_EmissionColor", Color.clear);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = false;
                //Debug.Log("Kinematic false");

                StartCoroutine(FakeOnSleep());
            }
        }
    }

    public bool Select()
    {
        selected = false;
        // Aquí debería estar a kinematic siempre
        //rb.isKinematic = true;
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

    private IEnumerator FakeOnSleep()
    {
        yield return waitSleep;

        rb.isKinematic = true;
        //Debug.Log("Kinematic true");
    }
}
