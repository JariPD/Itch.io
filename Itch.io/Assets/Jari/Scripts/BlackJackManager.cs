using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackJackManager : MonoBehaviour
{
    [Header("Card Lists")]
    [SerializeField] private List<int> deck;
    [SerializeField] private List<int> usersCards;
    
    [Header("Card Settings")]
    [SerializeField] private int amountOfCardsInPlay = 52;
    [SerializeField] private int totalMaxValue;
    
    [Header("User Stats")]
    public int UserTotalCardValue;
   
    void Start()
    {
        FillList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CallCard();

        //lose check
        if (UserTotalCardValue > totalMaxValue)
            print("You lost, card value exceeded 21");
    }

    private void FillList()
    {
        for (int a = 0; a < 4; a++)
        {
            //fills the deck
            for (int b = 0; b < 10; b++)
            {
                deck.Add(b + 1);
            }

            for (int i = 0; i < 3; i++)
                deck.Add(10);
        }

        
    }

    public void CallCard()
    {
        //get random card
        var random = Random.Range(0, deck.Count);

        //give card to user
        usersCards.Add(deck.ElementAt(random));

        //upates pulled card UI
        UIManager.instance.UpdatePulledCardText(deck.ElementAt(random));

        //calculates the sum of the user his cards
        UserTotalCardValue = usersCards.Sum();

        //remove card from deck
        deck.RemoveAt(random);
    }

    public void Fold()
    {
        print("You folded");
    }
}
