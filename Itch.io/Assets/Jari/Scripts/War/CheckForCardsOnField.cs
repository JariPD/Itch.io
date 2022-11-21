using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForCardsOnField : MonoBehaviour
{
    public GameObject[] enemyDefendingRow, enemyAttackingRow;
    public GameObject[] playerDefendingRow, playerAttackingRow;
    public int amountOnEnemyDefendingRow, amountOnEnemyAttackingRow;
    public int amountOnPlayerDefendingRow, amountOnPlayerAttackingRow;

    


    public void CheckForPlayer()
    {
        for (int i = 0; i < playerAttackingRow.Length; i++)
        {
            if (playerAttackingRow[i].GetComponent<WarTile>().HasCard)
                amountOnPlayerAttackingRow++;
        }

        for (int i = 0; i < playerDefendingRow.Length; i++)
        {
            if (playerDefendingRow[i].GetComponent<WarTile>().HasCard)
                amountOnPlayerDefendingRow++;
        }
    }

    public void CheckForAI()
    {
        for (int i = 0; i < enemyAttackingRow.Length; i++)
        {
            if (enemyAttackingRow[i].GetComponent<OpponentWarTile>().HasCard)
                amountOnEnemyAttackingRow++;
        }

        for (int i = 0; i < enemyDefendingRow.Length; i++)
        {
            if (enemyDefendingRow[i].GetComponent<OpponentWarTile>().HasCard)
                amountOnEnemyDefendingRow++;
        }
    }
}
