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
        Player newPlayer = Instantiate(playerPrefab, transform).GetComponent<Player>();
        newPlayer.Init(name, index, PhotonNetwork.MasterClient.ActorNumber == index, GameController.instance.GetPlayerColor(index));
        while(index - 1 >= players.Count)   // indices might not be in order
            players.Add(null);
        players[index - 1] = newPlayer;
    }

    public void RemovePlayer(int index, string name)
    {
        Destroy(players[index - 1].gameObject);
    }
}
