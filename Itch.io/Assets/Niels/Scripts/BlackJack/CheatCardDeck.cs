using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCardDeck : MonoBehaviour
{
    public static CheatCardDeck instance;

    [Header("Card Info")]
    public List<Transform> cardPos = new List<Transform>();
    [SerializeField] private int cardState = 0;

    [SerializeField] private Animator deckAnim;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        deckAnim.SetInteger("Cards", cardState);
    }

    public void AddCard()
    {
        cardState += 1;
    }

    public void MinusCard()
    {
        cardState -= 1;
    }
}
