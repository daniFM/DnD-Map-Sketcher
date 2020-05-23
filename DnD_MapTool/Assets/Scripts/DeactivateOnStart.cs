using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
