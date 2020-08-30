using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public RangeFloat spinForce;

    private Rigidbody rb;

    #region DEBUG

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Roll();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Roll();
        }
    }

    #endregion

    public void Roll()
    {
        transform.Translate(0, 10, 0, Space.World);
        rb.AddTorque(spinForce.GetRandom(), spinForce.GetRandom(), spinForce.GetRandom(), ForceMode.Impulse);
    }
}
