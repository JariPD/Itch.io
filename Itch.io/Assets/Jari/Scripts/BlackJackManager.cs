using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlackJackManager : MonoBehaviour
{
    [Header("Card info")]
    [SerializeField] private GameObject Card;

    [Header("Card Lists")]
    [SerializeField] private List<int> deck;
    [SerializeField] private List<int> usersCards;
    [SerializeField] private List<int> opponentsCards;
    
    [Header("Card Settings")]
    [SerializeField] private int amountOfCardsInPlay = 52;
    [SerializeField] private int totalMaxValue;

    [Header("Button Info")]
    [SerializeField] private GameObject callButton;
    [SerializeField] private GameObject foldButton;
    [SerializeField] private GameObject retryButton;

    [Header("State Info")]
    [SerializeField] private Button call;
    [SerializeField] private Button fold;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject draw;

    [Header("User Stats")]
    public int UserTotalCardValue;

    [Header("Opponent Stats")]
    public int OpponentTotalCardValue;

    void Start()
    {
        FillList();
    }

    void Update()
    {
        //state lose/win check
        if (UserTotalCardValue > totalMaxValue)
            Lose();
        if (OpponentTotalCardValue > totalMaxValue)
            Win();
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

    public void Call()
    {
        //turn based button first player then opponent(AI)
        fold.interactable = true;
        StartCoroutine(TurnBase());
    }

    private void PlayerTurn()
    {
        CallCard();
    }

    private void OpponentTurn()
    {
        CallOpponentCard(10, 0, 0.1f);
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

        //add Card To Game With Value
        GameObject card = Instantiate(Card);
        for (int i = 0; i < usersCards.Count; i++)
            card.transform.position = new Vector3(-10f + i + 5, 0, 0.1f + i);

        card.GetComponent<BlackJackCard>().cardNumber = deck.ElementAt(random);

        //remove card from deck
        deck.RemoveAt(random);
    }

    public void CallOpponentCard(float x, float y, float z)
    {
        //get random card
        var random = Random.Range(0, deck.Count);

        //give card to user
        opponentsCards.Add(deck.ElementAt(random));

        //upates pulled card UI
        UIManager.instance.UpdateOpponentPulledCardText(deck.ElementAt(random));

        //calculates the sum of the user his cards
        OpponentTotalCardValue = opponentsCards.Sum();

        //add Card To Game With Value
        GameObject card = Instantiate(Card);
        for (int i = 0; i < opponentsCards.Count; i++)
            card.transform.position = new Vector3(x - i - 5, y, z + i);

        card.GetComponent<BlackJackCard>().cardNumber = deck.ElementAt(random);

        //remove card from deck
        deck.RemoveAt(random);

        //opponent always wins if max value is reached
        if (OpponentTotalCardValue == totalMaxValue)
            Lose();
    }

    public void Win()
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
        //end of the game retry button pops up
        callButton.SetActive(false);
        foldButton.SetActive(false);
        retryButton.SetActive(true);
    }

    public void Fold()
    {
        print("You folded");
        ButtonSwitch();
        
        UIManager.instance.UpdateOpponentPulledCardText(OpponentTotalCardValue);

        //if opponent has more then 17 total points he choses if he plays on after player has folded out
        if (OpponentTotalCardValue > 17)
        {
            int change = Random.Range(0, 2);
            if (change == 0)
                StartCoroutine(OpponentPlaysOn());
            else if (change == 1)
                CheckWhoWins();
        }
        else if (OpponentTotalCardValue <= 17)
            StartCoroutine(OpponentPlaysOn());
    }

    private void CheckWhoWins()
    {
        //if player and opponent both have the max value opponent always wins: player loses 
        if (UserTotalCardValue == totalMaxValue && OpponentTotalCardValue == totalMaxValue)
            Lose();
        if (OpponentTotalCardValue == totalMaxValue)
            Lose();

        //if player has equal points as opponent: draw match
        if (UserTotalCardValue == OpponentTotalCardValue)
            Draw();

        //check if opponent and player are below max value
        if (OpponentTotalCardValue < totalMaxValue && UserTotalCardValue < totalMaxValue)
        {
            //check if opponent has more points below max value then player: player loses
            //else player wins but first check if opponent doesn't have the same points as player
            if (OpponentTotalCardValue > UserTotalCardValue)
                Lose();
            else if (OpponentTotalCardValue != UserTotalCardValue)
                Win();
        }
        else if (OpponentTotalCardValue > totalMaxValue && UserTotalCardValue < totalMaxValue)
        {
            //check if opponent is above max value and player below: player wins
            Win();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    private void InteractableSwitch()
    {
        //switching state to opposite
        fold.interactable = !fold.interactable;
        call.interactable = !call.interactable;
    }

    private IEnumerator OpponentPlaysOn()
    {
        //if player isn't above opponent opponent get calls more cards else check if opponent wins
        if (UserTotalCardValue !> OpponentTotalCardValue)
            CallOpponentCard(10, 0, 0.1f);
        if (OpponentTotalCardValue >= UserTotalCardValue)
            CheckWhoWins();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(OpponentPlaysOn());
    }

    public IEnumerator TurnBase()
    {
        //first call card for player and then if AI wants he can also call a card
        InteractableSwitch();
        PlayerTurn();
        yield return new WaitForSeconds(0.7f);
        if (OpponentTotalCardValue <= 18 && UserTotalCardValue < 22)
        {
            OpponentTurn();
            InteractableSwitch();
        }
        else
        {
            InteractableSwitch();
        }
    }
}