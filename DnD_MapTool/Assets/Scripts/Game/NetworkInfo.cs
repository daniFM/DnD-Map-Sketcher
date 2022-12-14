// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//[Serializable] public class TileData: SDictionary<TileType, Transform>
//{
//    public TileData(TileData copy)
//    {
//        keys = new List<TileType>(copy.Keys);
//        values = new List<Transform>(copy.values);
//    }
//}
//[Serializable] public class TokenData: SDictionary<string, Transform> { }

//public class NetworkInfo : MonoBehaviour, IPunObservable
//{
//    public TileData tileData;
//    public TokenData tokenData;

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        GameController.OnTilesUpdated?.Invoke();
//        Debug.Log("Esto hace algo?");
//    }
//}