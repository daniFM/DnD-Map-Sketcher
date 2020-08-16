using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TokenController: MonoBehaviour
{
    [SerializeField] private GameObject tokenPrefab;
    public LayerMask tokenLayer;

    private bool active;
    private Token activeToken;

    void Start()
    {
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
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, tokenLayer))
                {
                    //Debug.Log("Trying to select " + hit.collider.name);
                    activeToken = hit.collider.GetComponent<Token>();
                    if(!activeToken.Select())
                    {
                        activeToken = null;
                        //Debug.Log("Could not select");
                    }
                }
            }
        }
    }

    public void CreateToken()
    {
        PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.ActorNumber });
    }

    public void CreateToken(int player)
    {
        PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { player });
    }

    public void CreateToken(Color color)
    {
        PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, color.r, color.g, color.b });
    }

    public void CreateToken(int player, Color color)
    {
        PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { player, color });
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