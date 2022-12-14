// Copyright (c) 2022 Daniel Fernández Marqués

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ping : MonoBehaviourPun
{
    public bool overrideColor;
    public Color colorOverride;
    public bool multiplyColor;
    public Color colorMultiply;
    public bool addColor;
    public Color colorAdd;

    public float cameraDistance = 5;

    void Start()
    {
        Color color;
        if(overrideColor)
            color = colorOverride;
        else
            color = GameController.instance.GetPlayerColor(photonView.OwnerActorNr);

        if(multiplyColor)
            color *= colorMultiply;

        if(addColor)
            color += colorAdd;

        // After the recent update this is done via:
        ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
        settings.startColor = color;

        transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, cameraDistance);
    }
}
