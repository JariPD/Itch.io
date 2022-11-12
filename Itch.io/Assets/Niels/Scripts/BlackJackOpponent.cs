using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackOpponent : MonoBehaviour
{
    public static BlackJackOpponent instance;

    public BlackJackManager blackJackManager;

    [Header("Card Info")]
    public List<Transform> cardPos = new List<Transform>();
    [SerializeField] private int cardState = 0;

    [SerializeField] private Animator deckAnim;

    private void Start()
    {
        instance = this;

        if (deckAnim != null)
            deckAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        deckAnim.SetInteger("Cards", cardState);
    }

    public void AddCard()
    {
        cardState += 1;
    }
    public void ResetState()
    {
        cardState = 0;
    }
}
