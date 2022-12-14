// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TileSetter : MonoBehaviour
//{
//    public GameObject tilePrefab;

//    private TileData prevTileData;

//    void OnEnable()
//    {
//        GameController.OnTilesUpdated += SetTiles;
//    }

//    void OnDisable()
//    {
//        GameController.OnTilesUpdated += SetTiles;
//    }

//    private void SetTiles()
//    {
//        TileData newTileData = GameController.instance.netInfo.tileData;
//        for(int i = 0; i < newTileData.Count; ++i)
//        {
//            Destroy(prevTileData.ValueAt(i).gameObject);
//            if(newTileData.KeyAt(i) != prevTileData.KeyAt(i))
//            {
//                Tile tile = Instantiate(tilePrefab, newTileData.ValueAt(i).position, Quaternion.identity).GetComponent<Tile>();
//                tile.SetTile(newTileData.KeyAt(i));
//            }
//        }
//        prevTileData = new TileData(newTileData);
//    }
//}
