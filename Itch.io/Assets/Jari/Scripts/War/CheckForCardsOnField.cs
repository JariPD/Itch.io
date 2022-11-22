using UnityEngine;

public class CheckForCardsOnField : MonoBehaviour
{
    public GameObject[] AIDefendingRow, AIAttackingRow;
    public GameObject[] PlayerDefendingRow, PlayerAttackingRow;
    public int AIDefendingCount, AIAttackingCount;
    public int DefendingCount, AttackingCount;

    public void CheckForPlayer()
    {
        DefendingCount = 0;
        AttackingCount = 0;

        for (int i = 0; i < PlayerAttackingRow.Length; i++)
        {
            if (PlayerAttackingRow[i].GetComponent<WarTile>().HasCard)
                AttackingCount++;
        }

        for (int i = 0; i < PlayerDefendingRow.Length; i++)
        {
            if (PlayerDefendingRow[i].GetComponent<WarTile>().HasCard)
                DefendingCount++;
        }
    }

    public void CheckForAI()
    {
        AIDefendingCount = 0;
        AIAttackingCount = 0;

        for (int i = 0; i < AIAttackingRow.Length; i++)
        {
            if (AIAttackingRow[i].GetComponent<OpponentWarTile>().HasCard)
                AIAttackingCount++;
        }

        for (int i = 0; i < AIDefendingRow.Length; i++)
        {
            if (AIDefendingRow[i].GetComponent<OpponentWarTile>().HasCard)
                AIDefendingCount++;
        }
    }
}
