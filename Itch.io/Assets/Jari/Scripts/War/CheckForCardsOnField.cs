using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForCardsOnField : MonoBehaviour
{
    public GameObject[] AIDefendingRow, AIAttackingRow;
    public GameObject[] PlayerDefendingRow, PlayerAttackingRow;
    public int AIDefendingCount, AIAttackingCount;
    public int DefendingCount, AttackingCount;
    public int PlayerWinCount, AIWinCount;

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
            PlayerWinCount++;
            UIManager.instance.UpdateWarWinCountText();
            UIManager.instance.WarGameResults(playerWon: false);

            if (AIWinCount >= 3)
                StartCoroutine(WinOrLose(win: false));
        }

        //check if AI still has cards if no cards player won
        CheckForAI();
        if (AIAttackingCount + AIDefendingCount <= 0)
        {
            PlayerWinCount++;
            UIManager.instance.UpdateWarWinCountText();
            UIManager.instance.WarGameResults(playerWon: true);

            if (PlayerWinCount >= 3)
                StartCoroutine(WinOrLose(win: true));
        }
    }

    IEnumerator WinOrLose(bool win)
    {
        yield return new WaitForSeconds(3f);

        if (win)
            SceneManager.LoadScene("Russian Roulette");
        else
            SceneManager.LoadScene("War");
    }
}
