// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType
{
    D4 = 4,
    D6 = 6,
    D8 = 8,
    D10 = 10,
    D20 = 20,
    D100 = 100
}

public class DiceController : MonoBehaviour
{
    [SerializeField] private GameObject[] dicePrefabs;
    [SerializeField] private Transform diceSpawn;

    private int diceQueue;
    private List<int> results;
    //private Dice[,] dicePool;
    private string[] dicePrefabNames;
    private readonly Dictionary<DiceType, int> diceLookup = new Dictionary<DiceType, int>
    {
        { DiceType.D4,   0 },
        { DiceType.D6,   1 },
        { DiceType.D8,   2 },
        { DiceType.D10,  3 },
        { DiceType.D20,  4 },
        { DiceType.D100, 5 }
    };
    private System.Text.StringBuilder sb;

    void Start()
    {
        results = new List<int>();

        dicePrefabNames = new string[dicePrefabs.Length];
        for(int i = 0; i < dicePrefabs.Length; ++i)
        {
            dicePrefabNames[i] = dicePrefabs[i].name;
        }

        //dicePool = new Dice[5, 5];
        sb = new System.Text.StringBuilder();

        //for(int i = 0; i < dicePrefabs.Length; ++i)
        //{
        //    for(int j = 0; j < dicePool.GetLength(1); ++j)
        //    {
        //        dicePool[i, j] = Instantiate(dicePrefabs[i], null).GetComponent<Dice>();
        //    }
        //}
    }

    public void Roll(DiceType dice, int number = 1)
    {
        Dice.Rolled += OnRollResult;

        diceQueue = number;
        int diceIndex = diceLookup[dice];

        for(int i = 0; i < number; ++i)
        {
            //for(int j = 0; j < dicePool.GetLength(1); ++j)
            //{
            //    Dice rdice = dicePool[diceIndex, j];
            //    if(!rdice.gameObject.activeSelf)
            //    {
            //        rdice.Roll(diceSpawn.position);
            //        break;
            //    }
            //}
            Photon.Pun.PhotonNetwork.Instantiate(dicePrefabNames[diceIndex], diceSpawn.position, Quaternion.identity);
        }
    }

    private void OnRollResult(int result)
    {
        diceQueue--;
        results.Add(result);

        if(diceQueue == 0)
        {
            Dice.Rolled -= OnRollResult;

            sb.Length = 0;
            int total = 0;

            sb.Append("Dice result: ");
            foreach(int i in results)
            {
                total += i;
                sb.Append(i).Append(", ");
            }
            sb.Append("(").Append(total).Append(")");

            GameController.instance.chat.SendChatMessageToAll(sb.ToString(), false);

            results.Clear();
        }

        Debug.Log("Dice result: " + result);
    }
}
