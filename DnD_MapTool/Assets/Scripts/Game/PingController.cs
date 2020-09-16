using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingController : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Transform particleSpawn;
    public LayerMask layers;

    void Update()
    {
        if(GameController.instance.controls.GetKeyDown(8))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
            {
                transform.position = hit.point;
                PhotonNetwork.Instantiate(particlePrefab.name, particleSpawn.position, particleSpawn.rotation);
            }
            else
            {
                GameController.instance.Tooltip("Try hovering your mouse over the map and pressing " + GameController.instance.controls.keys[8].GetKeyCode() + " to ping in that position");
            }
        }
    }
}
