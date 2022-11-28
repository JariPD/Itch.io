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
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private TextMeshProUGUI opponentDiceRollText;

    [Header("Buttons")]
    [SerializeField] private Button throwDiceButton;
    [SerializeField] private Button turnButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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

    private IEnumerator TurnOffText(TextMeshProUGUI text, float time)
    {
        yield return new WaitForSeconds(time);
        text.enabled = false;
    }

    public void TurnButton(bool active)
    {
        turnButton.gameObject.SetActive(active);
    }

    public void DisableThrowDiceButton()
    {
        Destroy(throwDiceButton.gameObject);
    }
}
