// Copyright (c) Daniel Fernández 2022


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class Dice : MonoBehaviourPun
{
    [SerializeField] private RangeFloat spinForce;
    [SerializeField] private float timeDespawn = 1;

    [Header("Animation")]
    public bool doAnimation = true;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Ease easeType;
    [SerializeField] private float overshoot = 1.70158f;

    private bool instantiated;
    private Rigidbody rb;
    private Transform [] faces;
    private WaitForSeconds waitDespawn;
    private WaitForSeconds waitAnimation;

    public static Action<int> Rolled;

    private void Start()
    {
        if(!instantiated)
        {
            Instantiate();
        }
        Roll();
    }

    public void Instantiate()
    {
        rb = GetComponent<Rigidbody>();

        Transform[] transforms = transform.GetComponentsInChildren<Transform>();
        faces = new Transform[transforms.Length - 1];
        Array.Copy(transforms, 1, faces, 0, transforms.Length - 1);
        //faces = transform.GetComponentsInChildren<Transform>();

        waitDespawn = new WaitForSeconds(timeDespawn);
        waitAnimation = new WaitForSeconds(animationTime);

        gameObject.SetActive(false);

        //Roll();
        instantiated = true;
    }

    public void Roll(/*Vector3 position*/)
    {
        if(!instantiated)
        {
            Instantiate();
        }
        gameObject.SetActive(true);
        StartCoroutine(RollRoutine(/*position*/));
    }

    private IEnumerator RollRoutine(/*Vector3 position*/)
    {
        float offset = 3f;
        transform.position += new Vector3(UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset));
        //int iterations = 0;
        //do
        //{
        //    position += new Vector3(UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset));
        //    iterations++;
        //}
        //while(Physics.CheckSphere(position, 1f));
        //Debug.Log("Iterations: " + iterations);

        //transform.position = position;
        transform.localScale = Vector3.one;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //transform.Translate(0, 10, 0, Space.World);
        rb.AddTorque(spinForce.GetRandom(), spinForce.GetRandom(), spinForce.GetRandom(), ForceMode.Impulse);

        yield return new WaitUntil(() => rb.IsSleeping());

        float height = -Mathf.Infinity;
        int face = 0;
        for(int i = 0; i < faces.Length; ++i)
        {
            float fh = faces[i].position.y;
            if(fh > height)
            {
                height = fh;
                face = i;
            }
        }

        Rolled?.Invoke(face + 1);

        yield return waitDespawn;

        if(doAnimation)
        {
            photonView.RPC("DoAnimation", RpcTarget.All);
        }

        yield return waitAnimation;

        //gameObject.SetActive(false);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    public void DoAnimation()
    {
        transform.DOScale(0, animationTime).SetEase(easeType, overshoot);
    }
}
