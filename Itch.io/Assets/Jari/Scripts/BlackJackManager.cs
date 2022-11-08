using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackJackManager : MonoBehaviour
{
    [Header("Card info")]
    [SerializeField] private GameObject Card;

    [Header("Card Lists")]
    [SerializeField] private List<int> deck;
    [SerializeField] private List<int> usersCards;
    
    [Header("Card Settings")]
    [SerializeField] private int amountOfCardsInPlay = 52;
    [SerializeField] private int totalMaxValue;

    [Header("Button Info")]
    [SerializeField] private GameObject callButton;
    [SerializeField] private GameObject foldButton;
    [SerializeField] private GameObject retryButton;

    [Header("State Info")]
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject draw;

    [Header("User Stats")]
    public int UserTotalCardValue;
   
    void Start()
    {
        FillList();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    CallCard();

        //lose check
        if (UserTotalCardValue > totalMaxValue)
            Lose();
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
        var random = Random.Range(0, deck.Count -1);

        //give card to user
        usersCards.Add(deck.ElementAt(random));

        //upates pulled card UI
        UIManager.instance.UpdatePulledCardText(deck.ElementAt(random));

        //calculates the sum of the user his cards
        UserTotalCardValue = usersCards.Sum();

        //add Card To Game With Value
        GameObject card = Instantiate(Card);
        for (int i = 0; i < usersCards.Count; i++)
            card.transform.position = new Vector3(-10f + i + 5, 0, 0.1f + i);

        card.GetComponent<BlackJackCard>().cardNumber = deck.ElementAt(random);

        //remove card from deck
        deck.RemoveAt(random);
    }

    private void Win()
    {
        win.SetActive(true);
        ButtonSwitch();
    }

    private void Draw()
    {
        draw.SetActive(true);
        ButtonSwitch();
    }

    private void Lose()
    {
        lose.SetActive(true);
        ButtonSwitch();
    }

    private void ButtonSwitch()
    {
        callButton.SetActive(false);
        foldButton.SetActive(false);
        retryButton.SetActive(true);
    }

    public void Fold()
    {
        print("You folded");
        callButton.SetActive(false);
        foldButton.SetActive(false);
        
        int opponentValue = Random.Range(18, 24);
        UIManager.instance.UpdateOpponentPulledCardText(opponentValue);
        if (opponentValue > totalMaxValue)
            Win();
        else if (opponentValue == UserTotalCardValue)
            Draw();
        else if (opponentValue < UserTotalCardValue)
            Win();
        else
            CheckWhoWins(opponentValue);

        if (opponentValue != UserTotalCardValue)
        {
            if (opponentValue == totalMaxValue)
                Lose();
        }
    }

    private void CheckWhoWins(int _opponentValue)
    {
        if (_opponentValue < totalMaxValue && UserTotalCardValue < totalMaxValue)
        {
            if (_opponentValue > UserTotalCardValue)
                Lose();
            else
                Win();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
