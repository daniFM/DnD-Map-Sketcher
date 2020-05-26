using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED
public class Player : MonoBehaviour
{
    //public string playerName;
    public string characterName;
    public List<Token> assignedTokens;

    public void Init(/*string playerName, */GameObject tokenPrefab, string characterName = "")
    {
        this.characterName = characterName;
        assignedTokens = new List<Token>();
        assignedTokens.Add(PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Token>());
    }
}
