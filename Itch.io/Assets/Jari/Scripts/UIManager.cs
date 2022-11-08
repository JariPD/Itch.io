using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        userCardValueText.text = "Cards value: " + blackJackManager.UserTotalCardValue.ToString();
    }
    
    public void UpdatePulledCardText(int cardValue)
    {
        pulledCardText.text = "You pulled: " + cardValue.ToString();
    }

    public void UpdateOpponentPulledCardText(int opponentValue)
    {
        opponentCardValueText.text = "Opponent Got: " + opponentValue.ToString();
    }
}
