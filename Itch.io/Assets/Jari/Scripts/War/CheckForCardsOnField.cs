using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    /// <summary> 
    ///   check if player still has cards on the field if no cards AI won
    /// </summary>
    public void WarWinCheck()
    {
        //check if player still has cards if no cards AI won
        CheckForPlayer();
        if (AttackingCount + DefendingCount <= 0)
        {
            UIManager.instance.WarGameResults(playerWon: false);
            StartCoroutine(WinOrLose(win: false));
        }

        //check if AI still has cards if no cards player won
        CheckForAI();
        if (AIAttackingCount + AIDefendingCount <= 0)
        {
            UIManager.instance.WarGameResults(playerWon: true);
            StartCoroutine(WinOrLose(win: true));
        }
    }

    IEnumerator WinOrLose(bool win)
    {
        yield return new WaitForSeconds(3.1f);

        if (win)
            SceneManager.LoadScene("Russian Roulette");
        else
            SceneManager.LoadScene("War");
    }
}
