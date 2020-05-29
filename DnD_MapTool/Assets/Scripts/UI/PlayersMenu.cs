﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayersMenu : MonoBehaviour
{
    public GameObject playerPrefab;
    [SerializeField] private Color[] colors;

    private List<Player> players;

    private void Start()
    {
        players = new List<Player>();

        AddPlayer(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);

        foreach(Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerListOthers)
        {
            AddPlayer(otherPlayer.ActorNumber, otherPlayer.NickName);
        }
    }

    private void OnEnable()
    {
        NetworkManager.OnPlayerJoined += AddPlayer;
        NetworkManager.OnPlayerLeft += RemovePlayer;
    }

    private void OnDisable()
    {
        NetworkManager.OnPlayerJoined -= AddPlayer;
        NetworkManager.OnPlayerLeft -= RemovePlayer;
    }

    public void AddPlayer(int index, string name)
    {
        Color playerColor;

        index -= 1;
        if(index < colors.Length)
            playerColor = colors[index];
        else
            playerColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));

        Player newPlayer = Instantiate(playerPrefab, transform).GetComponent<Player>();
        newPlayer.Init(name, playerColor);
        while(index >= players.Count)   // indices might not be in order
            players.Add(null);
        players[index] = newPlayer;
    }

    public void RemovePlayer(int index, string name)
    {
        Destroy(players[index-1].gameObject);
    }
}
