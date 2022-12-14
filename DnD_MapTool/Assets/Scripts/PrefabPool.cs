// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabPool: MonoBehaviour
{
    public List<GameObject> Prefabs;

    void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if(pool != null && this.Prefabs != null)
        {
            foreach(GameObject prefab in this.Prefabs)
            {
                if(!pool.ResourceCache.ContainsKey(prefab.name))
                    pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
    }
}