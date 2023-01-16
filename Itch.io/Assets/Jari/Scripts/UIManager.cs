using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    [SerializeField] private BlackJackManager blackJackManager;
    [SerializeField] private WarManager warManager;

    [Header("Black Jack Text Objects")]
    [SerializeField] private TextMeshProUGUI userCardValueText;
    [SerializeField] private TextMeshProUGUI opponentCardValueText;
    [SerializeField] private TextMeshProUGUI opponentMatchPoints;
    [SerializeField] private TextMeshProUGUI playerMatchPoints;

    [Header("War Text Objects")]
    [SerializeField] private TextMeshProUGUI warGameResults;
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private TextMeshProUGUI opponentDiceRollText;
    [SerializeField] private TextMeshProUGUI playerHealth, enemyHealth;

    [Header("War Tutorial")]
    [SerializeField] private GameObject focusPanel;
    [SerializeField] private GameObject attackingRowPanel, defendingRowPanel;
    [SerializeField] private GameObject placeCardText;

    [Header("Buttons")]
    [SerializeField] private Button throwDiceButton;
    [SerializeField] private Button turnButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (opponentMatchPoints != null && playerMatchPoints != null)
        {
            opponentMatchPoints.text = "Opponent Points: " + blackJackManager.PlayerPoints;
            playerMatchPoints.text = "Player Points: " + blackJackManager.PlayerPoints;
        }
    }

    void Update()
    {
        //updates total card values text
        if (blackJackManager != null)
        {
            userCardValueText.text = "" + blackJackManager.UserTotalCardValue.ToString();
            opponentCardValueText.text = "Opponents value: " + blackJackManager.OpponentTotalCardValue.ToString();
        }
    }

    public void UpdateScore(bool PlayerOpponnent)
    {
        if (PlayerOpponnent)
        {
            StartCoroutine(PlayerScoreChange());
        }
        else
        {
            StartCoroutine(OpponentScoreChange());
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
            opponentDiceRollText.text = "??? rolled: " + diceValue.ToString();
        }
    }

    public void UpdateWarHealth(int player, int enemy)
    {
        if (playerHealth.text != null)
            playerHealth.text = "" + player;

        if (enemyHealth.text != null)
            enemyHealth.text = "" + enemy;
    }

    public void TurnButton(bool active)
    {
        turnButton.gameObject.SetActive(active);
    }

    public void DisableThrowDiceButton()
    {
        if (throwDiceButton != null)
            Destroy(throwDiceButton.gameObject);
    }

    #endregion

    private IEnumerator TurnOffText(TextMeshProUGUI text, float time)
    {
        yield return new WaitForSeconds(time);
        text.enabled = false;
    }

    private IEnumerator PlayerScoreChange()
    {
        playerMatchPoints.gameObject.GetComponent<Animator>().Play("ScoreChange");
        yield return new WaitForSeconds(0.5f);
        if (opponentMatchPoints != null && playerMatchPoints != null)
        {
            playerMatchPoints.text = "Player Points: " + blackJackManager.PlayerPoints;
        }
    }

    private IEnumerator OpponentScoreChange()
    {
        yield return new WaitForSeconds(0.5f);
        opponentMatchPoints.gameObject.GetComponent<Animator>().Play("ScoreChange");
        yield return new WaitForSeconds(0.5f);
        if (opponentMatchPoints != null && playerMatchPoints != null)
        {
            opponentMatchPoints.text = "Opponent Points: " + blackJackManager.OpponentPoints;
        }
    }
}
