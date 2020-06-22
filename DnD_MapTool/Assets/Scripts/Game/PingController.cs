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
        if(Input.GetKeyDown(KeyCode.P))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
            {
                transform.position = hit.point;
                PhotonNetwork.Instantiate(particlePrefab.name, particleSpawn.position, particleSpawn.rotation);
            }
        }
    }
}
