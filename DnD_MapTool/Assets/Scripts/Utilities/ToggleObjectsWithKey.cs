using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectsWithKey : MonoBehaviour
{
    public KeyCode key;
    public GameObject[] objects;

    private bool active = true;

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            ToggleChildren();
        }
    }

    void ToggleChildren()
    {
        active = !active;
        foreach(GameObject go in objects)
        {
            go.SetActive(active);
        }
    }
}
