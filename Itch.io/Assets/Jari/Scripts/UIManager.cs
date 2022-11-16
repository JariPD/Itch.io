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
    [SerializeField] private TextMeshProUGUI pulledCardText;
    [SerializeField] private TextMeshProUGUI opponentpulledCardText;

    [Header("War Text Objects")]
    [SerializeField] private TextMeshProUGUI diceRollText;
    [SerializeField] private TextMeshProUGUI opponentDiceRollText;

    [Header("War Buttons")]
    [SerializeField] private Button warButton;

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
    }

    public void UpdatePulledCardText(int cardValue)
    {
        pulledCardText.text = "You pulled: " + cardValue.ToString();
    }

    public void UpdateOpponentPulledCardText(int opponentValue)
    {
        opponentpulledCardText.text = "Opponent Got: " + opponentValue.ToString();
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

    public void DisableThrowDiceButton()
    {
        Destroy(warButton.gameObject);
    }
}
