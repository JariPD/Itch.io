using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForCardsOnField : MonoBehaviour
{
    public GameObject[] AIRow, PlayerRow;           
    public int AIDefendingCount, AIAttackingCount;
    public int DefendingCount, AttackingCount;
    public int PlayerWinCount, AIWinCount;

    /// <summary>
    /// function to check how many cards player has on the field
    /// </summary>
    public void CheckForPlayer()
    {
        DefendingCount = 0;
        AttackingCount = 0;

        for (int i = 0; i < PlayerRow.Length; i++)
        {
            if (PlayerRow[i].GetComponent<WarTile>().HasCard)
                AttackingCount++;
        }
    }

    /// <summary>
    /// function to check how many cards ai has on the field
    /// </summary>
    public void CheckForAI()
    {
        AIDefendingCount = 0;
        AIAttackingCount = 0;

        for (int i = 0; i < AIRow.Length; i++)
        {
            if (AIRow[i].GetComponent<OpponentWarTile>().HasCard)
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
            //get current ai win count
            AIWinCount = PlayerPrefs.GetInt("AIWinCount", AIWinCount);

            //add 1 to ai win count
            AIWinCount++;

            //save ai win count
            PlayerPrefs.SetInt("AIWinCount", AIWinCount);

            //update win count text
            UIManager.instance.UpdateWarWinCountText();

            //show game results
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
            //get current player win count
            PlayerWinCount = PlayerPrefs.GetInt("PlayerWinCount", PlayerWinCount);

            //add 1 to player win count
            PlayerWinCount++;

            //save player win count
            PlayerPrefs.SetInt("PlayerWinCount", PlayerWinCount);

            //update win count text
            UIManager.instance.UpdateWarWinCountText();

            //show game results
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

    /// <summary>
    /// call this function when player or AI wins 3 times
    /// </summary>
    /// <param name="win"></param>
    /// <returns></returns>
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
