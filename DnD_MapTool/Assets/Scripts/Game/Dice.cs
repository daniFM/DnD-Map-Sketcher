using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private RangeFloat spinForce;
    [SerializeField] private float timeDespawn = 1;

    private bool instantiated;
    private Rigidbody rb;
    private Transform [] faces;
    private WaitForSeconds waitDespawn;

    public static Action<int> Rolled;

    private void Start()
    {
        if(!instantiated)
        {
            Instantiate();
        }
    }

    public void Instantiate()
    {
        rb = GetComponent<Rigidbody>();

        Transform[] transforms = transform.GetComponentsInChildren<Transform>();
        faces = new Transform[transforms.Length - 1];
        Array.Copy(transforms, 1, faces, 0, transforms.Length - 1);
        //faces = transform.GetComponentsInChildren<Transform>();

        waitDespawn = new WaitForSeconds(timeDespawn);

        gameObject.SetActive(false);

        //Roll();
        instantiated = true;
    }

    public void Roll()
    {
        if(!instantiated)
        {
            Instantiate();
        }
        gameObject.SetActive(true);
        StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        transform.Translate(0, 10, 0, Space.World);
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

        gameObject.SetActive(false);
    }
}
