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

    private int diceQueue;
    private Dice[,] dicePool;
    private readonly Dictionary<DiceType, int> diceLookup = new Dictionary<DiceType, int>
    {
        { DiceType.D4,   0 },
        { DiceType.D6,   1 },
        { DiceType.D8,   2 },
        { DiceType.D10,  3 },
        { DiceType.D20,  4 },
        { DiceType.D100, 5 }
    };

    void Start()
    {
        dicePool = new Dice[5, 5];

        for(int i = 0; i < dicePrefabs.Length; ++i)
        {
            for(int j = 0; j < dicePool.GetLength(1); ++j)
            {
                dicePool[i, j] = Instantiate(dicePrefabs[i], null).GetComponent<Dice>();
            }
        }
    }

    #region DEBUG

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Dice.Rolled += OnRollResult;
            dicePool[0, 0].Roll();
        }
    }

    public void Roll(DiceType dice, int number = 1)
    {
        diceQueue = number;
        int diceIndex = diceLookup[dice];

        for(int i = 0; i < number; ++i)
        {
            Dice.Rolled += OnRollResult;
            for(int j = 0; j < dicePool.GetLength(1); ++j)
            {
                Dice rdice = dicePool[diceIndex, j];
                if(!rdice.gameObject.activeSelf)
                {
                    rdice.Roll();
                }
            }
            dicePool[0, 0].Roll();
        }
    }

    private void OnRollResult(int result)
    {
        diceQueue--;
        if(diceQueue == 0)
        {
            Dice.Rolled -= OnRollResult;
        }

        GameController.instance.chat.SendChatMessage("Dice result: " + result);
        Debug.Log("Dice result: " + result);
    }

    #endregion
}
