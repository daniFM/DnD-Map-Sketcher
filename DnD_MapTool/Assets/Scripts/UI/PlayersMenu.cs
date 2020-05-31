using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayersMenu : MonoBehaviour
{
    public GameObject playerPrefab;

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
        index -= 1;
        Player newPlayer = Instantiate(playerPrefab, transform).GetComponent<Player>();
        newPlayer.Init(name, PhotonNetwork.MasterClient.ActorNumber == index+1, GameController.instance.GetPlayerColor(index));
        while(index >= players.Count)   // indices might not be in order
            players.Add(null);
        players[index] = newPlayer;
    }

    public void RemovePlayer(int index, string name)
    {
        Destroy(players[index-1].gameObject);
    }
}
