// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnStart : MonoBehaviour
{
    public IManagedInstantiation[] preInstantiate;

    void Start()
    {
        if(preInstantiate != null)
        {
            foreach(IManagedInstantiation script in preInstantiate)
            {
                script.Instantiate();
            }
        }

        gameObject.SetActive(false);
    }
}
