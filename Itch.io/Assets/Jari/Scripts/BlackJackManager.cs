using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlackJackManager : MonoBehaviour
{
    public static BlackJackManager instance;

    [Header("Card Settings")]
    [SerializeField] private List<int> deck;
    [SerializeField] private List<int> usersCards;
    [SerializeField] private int amountOfCardsInPlay = 52;
    [SerializeField] private int userTotalCardValue;
    [SerializeField] private int totalMaxValue;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        FillList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CallCard();

        //lose check
        if (userTotalCardValue > totalMaxValue)
            print("You lost, card value exceeded 21");
    }

    private void FillList()
    {
        for (int a = 0; a < 4; a++)
        {
            //fills the deck
            for (int b = 0; b < 13; b++)
            {
                deck.Add(b + 1);
            }
        }
    }

    public void CallCard()
    {
        //get random card
        var random = Random.Range(0, deck.Count);

        //give card to user
        usersCards.Add(deck.ElementAt(random));

        //print called card
        print("you pulled: " + deck.ElementAt(random));

        //calculates the sum of the user his cards
        userTotalCardValue = usersCards.Sum();

        //remove card from deck
        deck.RemoveAt(random);
    }

    public void Fold()
    {
        //skips users turn
    }
}
