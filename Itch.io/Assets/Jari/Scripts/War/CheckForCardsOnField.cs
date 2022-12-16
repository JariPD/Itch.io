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
    public IEnumerator WarWinCheck()
    {
        //check if player still has cards if no cards AI won
        CheckForPlayer();
        if (AttackingCount + DefendingCount <= 0)
        {
            AIWinCount = PlayerPrefs.GetInt("AIWinCount", AIWinCount);
            AIWinCount++;
            PlayerPrefs.SetInt("AIWinCount", AIWinCount);
            UIManager.instance.UpdateWarWinCountText();
            UIManager.instance.WarGameResults(playerWon: false);

            if (AIWinCount >= 3)
                StartCoroutine(WinOrLose(win: false));
            else if (AIWinCount <= 2)
            {
                UIManager.instance.TurnButton(false);
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        //check if AI still has cards if no cards player won
        CheckForAI();
        if (AIAttackingCount + AIDefendingCount <= 0)
        {
            PlayerWinCount = PlayerPrefs.GetInt("PlayerWinCount", PlayerWinCount);
            PlayerWinCount++;
            PlayerPrefs.SetInt("PlayerWinCount", PlayerWinCount);
            UIManager.instance.UpdateWarWinCountText();
            UIManager.instance.WarGameResults(playerWon: true);

            if (PlayerWinCount >= 3)
                StartCoroutine(WinOrLose(win: true));
            else if (PlayerWinCount <= 2)
            {
                UIManager.instance.TurnButton(false);
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    IEnumerator WinOrLose(bool win)
    {
        yield return new WaitForSeconds(3f);

        if (win)
        {
            WarManager.instance.ClearPlayerPrefs();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            WarManager.instance.ClearPlayerPrefs();
            SceneManager.LoadScene("War");
        }
    }
}
