using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerDeck : MonoBehaviour
{
    public static PlayerDeck instance;

    public BlackJackManager blackJackManager;

    [Header("Card Info")]
    public List<Transform> cardPos = new List<Transform>();
    [SerializeField] private int cardState = 0;

    [SerializeField] private Animator deckAnim;
    public Animator PlayerDeckAnim;

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
    public void ResetState()
    {
        cardState = 0;
    }
}
