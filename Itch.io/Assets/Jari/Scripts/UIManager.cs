using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private BlackJackManager blackJackManager;
    [SerializeField] private WarManager warManager;
    [SerializeField] private CheckForCardsOnField checkForCardsOnField;

    [Header("Black Jack Text Objects")]
    [SerializeField] private TextMeshProUGUI userCardValueText;
    [SerializeField] private TextMeshProUGUI opponentCardValueText;
    [SerializeField] private TextMeshProUGUI opponentMatchPoints;
    [SerializeField] private TextMeshProUGUI playerMatchPoints;

    [Header("War Text Objects")]
    [SerializeField] private TextMeshProUGUI warGameResults;
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private TextMeshProUGUI opponentDiceRollText;
    [SerializeField] private TextMeshProUGUI playerWinCountText, AIWinCountText;

    [Header("War Tutorial")]
    [SerializeField] private GameObject focusPanel;
    [SerializeField] private GameObject attackingRowPanel, defendingRowPanel;
    [SerializeField] private GameObject placeCardText;
    private int focusTextCount = 0;
    private int warRowsCount = 0;

    [Header("Buttons")]
    [SerializeField] private Button throwDiceButton;
    [SerializeField] private Button turnButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        UpdateWarWinCountText();
    }

    void Update()
    {
        //updates total card values text
        if (blackJackManager != null)
        {
            userCardValueText.text = "Cards value: " + blackJackManager.UserTotalCardValue.ToString();
            opponentCardValueText.text = "Opponents value: " + blackJackManager.OpponentTotalCardValue.ToString();
        }

        if (opponentMatchPoints != null && playerMatchPoints != null)
        {
            opponentMatchPoints.text = "Opponent Points: " + blackJackManager.OpponentPoints;
            playerMatchPoints.text = "Player Points: " + blackJackManager.PlayerPoints;
        }
    }

    #region War

    public void UpdateDiceRollText(int diceValue, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            StartCoroutine(TurnOffText(diceRollText, 3));
            diceRollText.text = "You rolled: " + diceValue.ToString();
        }
        else
        {
            StartCoroutine(TurnOffText(opponentDiceRollText, 3));
            opponentDiceRollText.text = "Opponent rolled: " + diceValue.ToString();
        }
    }

    public void WarGameResults(bool playerWon)
    {
        if (playerWon)
            warGameResults.text = "You won the war!";
        else
            warGameResults.text = "You lost the war!";

        StartCoroutine(TurnOffText(warGameResults, 3));
    }

    public void TurnButton(bool active)
    {
        turnButton.gameObject.SetActive(active);
    }

    public void DisableThrowDiceButton()
    {
        Destroy(throwDiceButton.gameObject);
    }
    
    public void UpdateWarWinCountText()
    {
        if (playerWinCountText != null)
            playerWinCountText.text = "Player won: " + PlayerPrefs.GetInt("PlayerWinCount");

        if (AIWinCountText != null)
            AIWinCountText.text = "AI won: " + PlayerPrefs.GetInt("AIWinCount");
    }

    /*#region Tutorial
    public IEnumerator WarTutorialRows()
    {
        warRowsCount = PlayerPrefs.GetInt("WarTutorialRows", warRowsCount);

        if (warRowsCount <= 0)
        {
            StartCoroutine(TurnOffUIElements(20));

            //play defending row voiceline
            audioManager.Play("Defending");
            defendingRowPanel.SetActive(true);

            yield return new WaitForSeconds(7.5f);

            defendingRowPanel.SetActive(false);

            //play attacking row voiceline
            audioManager.Play("Attacking");
            attackingRowPanel.SetActive(true);

            yield return new WaitForSeconds(7.5f);

            attackingRowPanel.SetActive(false);

            audioManager.Play("Placing");
            placeCardText.SetActive(true);

            yield return new WaitForSeconds(5f);
            
            placeCardText.SetActive(false);

            TurnButton(true);

            warRowsCount++;
            PlayerPrefs.SetInt("WarTutorialRows", warRowsCount);
        }
    }

    public IEnumerator FocusTutorial()
    {
        //set playerpref
        focusTextCount = PlayerPrefs.GetInt("FocusTutorial", focusTextCount);
        if (focusTextCount <= 0)
        {
            audioManager.Play("Focus");
            focusPanel.SetActive(true);

            //starts coroutine to turn off ui elements to make tutorial more clear
            StartCoroutine(TurnOffUIElements(10));

            yield return new WaitForSeconds(5);

            audioManager.Play("Lose");

            yield return new WaitForSeconds(5);

            focusPanel.SetActive(false);
        }

        focusTextCount++;
        PlayerPrefs.SetInt("FocusTutorial", focusTextCount);
    }

    #endregion*/
    #endregion

    private IEnumerator TurnOffText(TextMeshProUGUI text, float time)
    {
        yield return new WaitForSeconds(time);
        text.enabled = false;
    }

    private IEnumerator TurnOffUIElements(float timer)
    {
        //match points
        playerWinCountText.enabled = false;
        AIWinCountText.enabled = false;

        //dice roll text
        opponentDiceRollText.enabled = false;
        diceRollText.enabled = false;

        yield return new WaitForSeconds(timer);

        //match points
        playerWinCountText.enabled = true;
        AIWinCountText.enabled = true;
    }
}
