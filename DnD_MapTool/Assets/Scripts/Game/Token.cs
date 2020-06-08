using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Token : MonoBehaviourPun
{
    public LayerMask movementLayers;
    public LayerMask collisionLayers;
    public float sleepTime = 1;
    public bool destroyOnPlayerLeave = true;

    private bool selected;
    private new Renderer renderer;
    private Material mainMaterial;
    private Rigidbody rb;
    private Collider collider;
    private WaitForSeconds waitSleep;
    private Photon.Realtime.Player player;
    private bool initialized;
    private bool canReposition = true;
    private bool justFell = false;
    private WaitForSeconds waitReposition = new WaitForSeconds(0.1f);
    private Tile groundTile;

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
        collider = GetComponent<Collider>();
        waitSleep = new WaitForSeconds(sleepTime);
        player = photonView.Owner;

        mainMaterial.SetColor("_BaseColor", color);

        initialized = true;

        Snap();
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

                Snap();

                mainMaterial.SetColor("_EmissionColor", Color.clear);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = false;
                //Debug.Log("Kinematic false");

                StartCoroutine(FakeOnSleep());
            }
        }
        else if(photonView.IsMine && canReposition)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, 0.05f, 0), 0.2f, collisionLayers);
            Tile t_tile = null;

            if(hitColliders.Length == 1)
            {
                Debug.Log("Repositioning because of no contacts");
                Reposition(false);
                groundTile = null;
                justFell = true;
            }
            else
            {
                for(int i = 0; i < hitColliders.Length; ++i)
                {
                    if(hitColliders[i] != collider)
                    {
                        int layer = hitColliders[i].gameObject.layer;

                        if(layer == LayerMask.NameToLayer("Token")) // Needs same logic as with tiles
                        {
                            Debug.Log("Repositioning because of collision with other Token");
                            Reposition(true);
                            break;
                        }
                        else if(layer == LayerMask.NameToLayer("Tile"))
                        {
                            t_tile = hitColliders[i].GetComponent<Tile>();

                            if(t_tile != groundTile && !justFell)
                            {
                                Debug.Log("Repositioning because of collision with a different tile");
                                Reposition(true);
                                groundTile = t_tile;
                                justFell = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Check if collision is contained in movement layers
    //    if(!selected &&
    //        movementLayers == (movementLayers | (1 << other.gameObject.layer)) &&
    //        canReposition)
    //    {
    //        Reposition();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    // Check if collision is contained in movement layers
    //    if(!selected &&
    //        movementLayers == (movementLayers | (1 << other.gameObject.layer)) &&
    //        canReposition)
    //    {
    //        Reposition();
    //    }
    //}

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

    private void Snap()
    {
        transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, Mathf.Floor(transform.position.z));
        if(!TileController.instance.snapToCenter)
            transform.Translate(0.5f, 0, 0.5f);
    }

    private void Reposition(bool jump)
    {
        if(jump)
            transform.position += Vector3.up * 1.5f;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        //Debug.Log("Kinematic false");

        StartCoroutine(FakeOnSleep());
    }

    private IEnumerator FakeOnSleep()
    {
        canReposition = false;

        yield return waitSleep;

        rb.isKinematic = true;
        //Debug.Log("Kinematic true");

        yield return waitReposition;

        canReposition = true;
    }

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
