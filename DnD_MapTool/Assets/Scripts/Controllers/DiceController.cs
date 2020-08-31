using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dice { D4, D6, D8, D10, D20, D100 }

public class DiceController : MonoBehaviour
{
    [SerializeField] private GameObject[] dicePrefabs;

    private Dice[,] dicePool;

    void Start()
    {
        dicePool = new Dice[5, 5];

        for(int i = 0; i < dicePrefabs.Length; ++i)
        {
            for(int j = 0; j < dicePool.GetLength(1); ++j)
            {
                dicePool[i, j] = Instantiate(dicePrefabs[i], null).GetComponent<Dice>();
                dicePool[i, j].gameObject.SetActive(false);
            }
        }
    }

    #region DEBUG

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            dicePool[0, 0].gameObject.SetActive(true);
            Dice.Rolled += OnRollResult;
            dicePool[0, 0].Roll();
        }
    }

    private void OnRollResult(int result)
    {
        Dice.Rolled -= OnRollResult;
        Debug.Log("Dice result: " + result);
    }

    #endregion
}
