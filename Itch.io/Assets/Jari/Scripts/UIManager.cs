using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    [SerializeField] private BlackJackManager blackJackManager;

    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI userCardValueText;
    [SerializeField] private TextMeshProUGUI opponentCardValueText;
    [SerializeField] private TextMeshProUGUI pulledCardText;
    [SerializeField] private TextMeshProUGUI opponentpulledCardText;

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
        userCardValueText.text = "Cards value: " + blackJackManager.UserTotalCardValue.ToString();
        opponentCardValueText.text = "Opponents value: " + blackJackManager.OpponentTotalCardValue.ToString();
    }
    
    public void UpdatePulledCardText(int cardValue)
    {
        pulledCardText.text = "You pulled: " + cardValue.ToString();
    }

    public void UpdateOpponentPulledCardText(int opponentValue)
    {
        opponentpulledCardText.text = "Opponent Got: " + opponentValue.ToString();
    }
}
