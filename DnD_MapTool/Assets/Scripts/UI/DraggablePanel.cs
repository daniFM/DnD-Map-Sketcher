using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler
{
    public float sensitivity = 1;

    //void Start()
    //{
        
    //}

    public void OnDrag(PointerEventData eventData)
    {
        transform.Translate(Input.GetAxis("Mouse X") * sensitivity, Input.GetAxis("Mouse Y") * sensitivity, 0);
    }
}
