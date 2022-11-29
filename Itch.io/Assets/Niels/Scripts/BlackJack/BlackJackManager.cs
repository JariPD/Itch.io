using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlackJackManager : MonoBehaviour
{
    public static BlackJackManager instance;

    [Header("Game Info")]
    public int PlayerPoints = 0;
    public int OpponentPoints = 0;

    [Header("Card Info")]
    [SerializeField] private GameObject Card;
    [SerializeField] private GameObject opponentCard;
    [SerializeField] private GameObject cheatCard;
    [SerializeField] private Transform cardDeckSpawn;
    [SerializeField] private Transform cardSpawn;

    [SerializeField] private Animator deckAnim;

    private int cardAmount = 0;
    private int opponentCardAmount = 0;

    [Header("Deck info")]
    [SerializeField] private List<GameObject> playerDeckObj;
    [SerializeField] private List<GameObject> OpponentDeckObj;

    [Header("Card Lists")]
    [SerializeField] private List<int> deck;
    [SerializeField] private List<int> usersCards;
    [SerializeField] private List<int> opponentsCards;
    [SerializeField] private List<GameObject> cheatCards;
    
    [Header("Card Settings")]
    //[SerializeField] private int amountOfCardsInPlay = 52;
    [SerializeField] private int totalMaxValue;
    [SerializeField] private BlackJackOpponent opponentDeck;

    [Header("Button Info")]
    [SerializeField] private GameObject callButton;
    [SerializeField] private GameObject foldButton;

    [Header("State Info")]
    [SerializeField] private Button call;
    [SerializeField] private Button fold;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject draw;

    public bool concluded = false;

    [Header("User Stats")]
    public int UserTotalCardValue;

    [Header("Opponent Stats")]
    public int OpponentTotalCardValue;

    private bool userOverMaxValue = false;
    private bool opponentOverMaxValue = false;

    private int cheatCardChance;
    private int cheatCardAmount = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreateDeck();
    }

    void Update()
    {
        //state lose/win check
        if (concluded)
            ResetAgain();

        if (PlayerPoints == 3)
            StartCoroutine(WonGame());
        else if (OpponentPoints == 3)
            StartCoroutine(LoseGame());
    }

    #region GameManager
    /// <summary>
    /// function to create a deck
    /// </summary>
    private void CreateDeck()
    {
        //fills deck with cards
        for (int a = 0; a < 4; a++)
        {
            for (int b = 0; b < 10; b++)
                deck.Add(b + 1);
        }
    }

    /// <summary>
    /// another update for the lists because coroutine didn't update the forloops rights
    /// </summary>
    private void ResetAgain()
    {
        if (playerDeckObj != null)
        {
            for (int i = 0; i < playerDeckObj.Count; i++)
            {
                Destroy(playerDeckObj[i]);
                playerDeckObj.Remove(playerDeckObj[i]);
            }
        }

        if (OpponentDeckObj != null)
        {
            for (int i = 0; i < OpponentDeckObj.Count; i++)
            {
                Destroy(OpponentDeckObj[i]);
                OpponentDeckObj.Remove(OpponentDeckObj[i]);
            }
        }

        if (usersCards != null)
            for (int i = 0; i < usersCards.Count; i++)
                usersCards.Remove(usersCards[i]);

        if (opponentsCards != null)
            for (int i = 0; i < opponentsCards.Count; i++)
                opponentsCards.Remove(opponentsCards[i]);

        if (playerDeckObj.Count == 0 && OpponentDeckObj.Count == 0)
            concluded = false;
    }

    /// <summary>
    /// resets all buttons, interactables, cards and lists for a new round
    /// </summary>
    private void ResetRound()
    {
        concluded = true;
        //ResetAgain();

        PlayerDeck.instance.ResetState();
        BlackJackOpponent.instance.ResetState();

        cardAmount = 0;
        opponentCardAmount = 0;

        UserTotalCardValue = 0;
        OpponentTotalCardValue = 0;
        callButton.SetActive(true);
        foldButton.SetActive(true);

        win.SetActive(false);
        lose.SetActive(false);
        draw.SetActive(false);

        fold.interactable = false;
        call.interactable = true;

        userOverMaxValue = false;
        opponentOverMaxValue = false;
    }

    /// <summary>
    /// puts all cards back to GameDeck
    /// </summary>
    private void BackToDeck()
    {
        for (int i = 0; i < opponentsCards.Count; i++)
        {
            deck.Add(opponentsCards[i]);
            opponentsCards.Remove(opponentsCards[i]);
        }
        for (int i = 0; i < usersCards.Count; i++)
        {
            deck.Add(usersCards[i]);
            usersCards.Remove(usersCards[i]);
        }
    }

    private void ButtonSwitch()
    {
        //End of the game retry button pops up
        callButton.SetActive(false);
        foldButton.SetActive(false);
    }

    //switches interactables to the state they are not
    private void InteractableSwitch()
    {
        //switching state to opposite
        fold.interactable = !fold.interactable;
        call.interactable = !call.interactable;
    }

    /// <summary>
    /// restarts round with dissolve effect en removes decks
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(Dissolver());
        BackToDeck();
        yield return new WaitForSeconds(1);
        ResetRound();
    }

    private IEnumerator WonGame()
    {
        print("You Win");
        //do animation or transition

        yield return new WaitForSeconds(5);
        //switch scene
        SceneManager.LoadScene("War");
    }

    private IEnumerator LoseGame()
    {
        print("You Lose");
        //do animation or transition

        yield return new WaitForSeconds(5);
        //switch scene
        SceneManager.LoadScene("BlackJack");
    }

    #endregion

    #region GameState
    /// <summary>
    /// win match
    /// </summary>
    public void Win()
    {
        StartCoroutine(RestartGame());
        PlayerPoints++;
        win.SetActive(true);
        ButtonSwitch();
    }

    /// <summary>
    /// draw match
    /// </summary>
    private void Draw()
    {
        StartCoroutine(RestartGame());
        draw.SetActive(true);
        ButtonSwitch();
    }

    /// <summary>
    /// lose match
    /// </summary>
    private void Lose()
    {
        StartCoroutine(RestartGame());
        OpponentPoints++;
        lose.SetActive(true);
        ButtonSwitch();
    }

    /// <summary>
    /// function to check who won
    /// </summary>
    private void WinCheck()
    {
        if (cheatCards.Count != 0)
            for (int i = 0; i < cheatCards.Count; i++)
                cheatCards[i].GetComponent<CheatCard>().UseAble = false;

        Debug.Log("WinCheck");

        if (UserTotalCardValue == totalMaxValue && OpponentTotalCardValue != totalMaxValue)
            Win();

        //if player and opponent both have the max value opponent always wins: player loses 
        if (UserTotalCardValue == totalMaxValue && OpponentTotalCardValue == totalMaxValue)
            Lose();
        if (OpponentTotalCardValue == totalMaxValue)
            Lose();

        //if player has equal points as opponent: draw match
        if (UserTotalCardValue == OpponentTotalCardValue && UserTotalCardValue < totalMaxValue && OpponentTotalCardValue < totalMaxValue)
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

    #endregion

    #region Player

    #region Calling
    /// <summary>
    /// function to be called from a button for calling a card
    /// </summary>
    public void Call()
    {
        //turn based button first player then opponent(AI)
        ChanceForCheatcard();
        fold.interactable = true;
        StartCoroutine(TurnBase());
    }

    /// <summary>
    /// calls card for player deck
    /// </summary>
    public void CallCard()
    {
        //get random card
        var random = Random.Range(0, deck.Count);

        //give card to user
        usersCards.Add(deck.ElementAt(random));

        //calculates the sum of the user his cards
        UserTotalCardValue += usersCards[usersCards.Count - 1];

        //add Card To Game With Value
        GameObject card = Instantiate(Card, cardSpawn.transform.position, cardSpawn.transform.rotation);
        playerDeckObj.Add(card);
        for (int i = 0; i < usersCards.Count; i++)
            card.GetComponent<BlackJackCard>().ownNumber = cardAmount;

        cardAmount++;

        card.GetComponent<BlackJackCard>().cardNumber = deck.ElementAt(random);

        //remove card from deck
        deck.RemoveAt(random);
    }

    public IEnumerator TurnBase()
    {
        if (cheatCards.Count != 0)
            for (int i = 0; i < cheatCards.Count; i++)
                cheatCards[i].GetComponent<CheatCard>().UseAble = false;
        //plays animation for picking up a card and switches buttons off
        PlayerDeck.instance.PlayerDeckAnim.Play("PlayerDeck");
        InteractableSwitch();
        yield return new WaitForSeconds(1);

        deckAnim.Play("Deck");
        yield return new WaitForSeconds(0.7f);

        //first call card for player and then if AI wants he can also call a card
        CallCard();

        PlayerDeck.instance.AddCard();

        if (UserTotalCardValue > totalMaxValue)
            userOverMaxValue = true;
        yield return new WaitForSeconds(1.5f);
        PlayerDeck.instance.PlayerDeckAnim.Play("PlayerDeckBack");

        yield return new WaitForSeconds(Random.Range(2, 4));
        if (OpponentTotalCardValue <= 18 && UserTotalCardValue < 22)
        {
            deckAnim.Play("Deck");
            yield return new WaitForSeconds(0.7f);
            CallOpponentCard();
            InteractableSwitch();
            opponentDeck.AddCard();
            if (OpponentTotalCardValue > totalMaxValue)
                opponentOverMaxValue = true;
        }
        else
        {
            InteractableSwitch();
        }
        yield return new WaitForSeconds(0.1f);
        if (userOverMaxValue && opponentOverMaxValue)
            Draw();
        else if (userOverMaxValue && opponentOverMaxValue == false)
            Lose();
        else if (userOverMaxValue == false && opponentOverMaxValue)
            Win();

        if (cheatCards.Count != 0)
            for (int i = 0; i < cheatCards.Count; i++)
                cheatCards[i].GetComponent<CheatCard>().UseAble = true;
    }
    #endregion

    #region Folding

    public void Fold()
    {
        print("You folded");
        ButtonSwitch();

        //if opponent has more then 17 total points he choses if he plays on after player has folded out
        if (OpponentTotalCardValue > 17)
        {
            int change = Random.Range(0, 2);
            if (change == 0)
            {
                StartCoroutine(OpponentPlaysOn());
                print("0");
            }
            else if (change == 1)
            {
                WinCheck();
                print("1");
            }
        }
        else if (OpponentTotalCardValue <= 17)
            StartCoroutine(OpponentPlaysOn());
    }

    #endregion

    #region CheatCards

    /// <summary>
    /// give a chance to getting a cheating card
    /// </summary>
    private void ChanceForCheatcard()
    {
        if (cheatCardChance == 0)
        {
            StartCoroutine(CheatCardAddDeck());
            cheatCardChance = Random.Range(3, 5);
        }
        else
            cheatCardChance--;
    }

    /// <summary>
    /// Use a cheat card to remove/add number to your total score
    /// </summary>
    public void UseCheatCard(GameObject _card)
    {
        CheatCardDeck.instance.MinusCard();
        cheatCardAmount--;
        UserTotalCardValue += _card.GetComponent<CheatCard>().cheatCardValue;
        StartCoroutine(CheatDissolve(_card));

        cheatCards.Remove(_card);
        if (UserTotalCardValue > totalMaxValue)
            WinCheck();
        else if (UserTotalCardValue == totalMaxValue)
            OpponentPlaysOn();
    }

    /// <summary>
    /// dissolving for cheat card being used
    /// </summary>
    private IEnumerator CheatDissolve(GameObject _cheatCard)
    {
        _cheatCard.GetComponent<CardShaderDissolve>().DissolveCard();
        yield return new WaitForSeconds(1);
        Destroy(_cheatCard);
    }
    public IEnumerator CheatCardAddDeck()
    {
        deckAnim.Play("Deck");
        CheatCardDeck.instance.AddCard();
        yield return new WaitForSeconds(0.7f);
        GameObject cheatcard = Instantiate(cheatCard);
        cheatCards.Add(cheatcard);
        cheatcard.GetComponent<CheatCard>().ownNumber = cheatCardAmount;
        cheatcard.GetComponent<CheatCard>().UseAble = false;
        cheatCardAmount++;
    }
    #endregion

    /// <summary>
    /// makes cards dissolve and removed from table and list
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dissolver()
    {
        Debug.Log("Dissolve");
        for (int i = 0; i < playerDeckObj.Count; i++)
            playerDeckObj[i].GetComponent<CardShaderDissolve>().DissolveCard();

        for (int i = 0; i < OpponentDeckObj.Count; i++)
            OpponentDeckObj[i].GetComponent<CardShaderDissolve>().DissolveCard();

        yield return new WaitForSeconds(1);

        for (int i = 0; i < playerDeckObj.Count; i++)
        {
            Destroy(playerDeckObj[i]);
            playerDeckObj.Remove(playerDeckObj[i]);
        }

        for (int i = 0; i < OpponentDeckObj.Count; i++)
        {
            Destroy(OpponentDeckObj[i]);
            OpponentDeckObj.Remove(OpponentDeckObj[i]);
        }
    }

    #endregion

    #region AI
    /// <summary>
    /// call card for opponent deck
    /// </summary>
    public void CallOpponentCard()
    {
        //get random card
        var random = Random.Range(0, deck.Count);

        //give card to user
        opponentsCards.Add(deck.ElementAt(random));

        //calculates the sum of the user his cards
        OpponentTotalCardValue = opponentsCards.Sum();

        //add Card To Game With Value
        GameObject card = Instantiate(opponentCard, cardSpawn.transform.position, cardSpawn.transform.rotation);
        OpponentDeckObj.Add(card);
        for (int i = 0; i < opponentsCards.Count; i++)
            card.GetComponent<BlackJackCardOpponent>().ownNumber = opponentCardAmount;

        opponentCardAmount++;

        card.GetComponent<BlackJackCardOpponent>().cardNumber = deck.ElementAt(random);

        //remove card from deck
        deck.RemoveAt(random);

        //opponent always wins if max value is reached
        if (OpponentTotalCardValue == totalMaxValue)
            Lose();

        if (UserTotalCardValue == totalMaxValue && OpponentTotalCardValue != totalMaxValue)
            WinCheck();
    }

    private IEnumerator OpponentPlaysOn()
    {
        Debug.Log("OpponentPlays");
        //if player isn't above opponent opponent get calls more cards else check if opponent wins
        if (UserTotalCardValue !> OpponentTotalCardValue)
        {
            deckAnim.Play("Deck");
            yield return new WaitForSeconds(0.7f);

            opponentDeck.AddCard();
            CallOpponentCard();
        }
        if (OpponentTotalCardValue >= UserTotalCardValue)
            WinCheck();

        //check if possible to play again
        if (!win.activeInHierarchy || !lose.activeInHierarchy || !draw.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(2, 6));
            if (OpponentTotalCardValue < UserTotalCardValue)
                StartCoroutine(OpponentPlaysOn());
        }
    }
    #endregion
}