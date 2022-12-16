using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WarManager : MonoBehaviour
{
    public static WarManager instance;

    public int diceRoll;
    private bool isPlayerTurn = true;
    private int turnCount = 0;

    [Header("References")]
    [SerializeField] private Transform[] cardSpawnPos;                  //spawn positions for cards
    [SerializeField] private GameObject card, opponentCard;             //cards prefabs
    private WarAI warAI;
    private CheckForCardsOnField checkForCardsOnField;

    [Header("Card Placement")]
    public GameObject CurrentSelectedCard;                              //reference to the card that is currently selected
    public bool PlacingCard = false;                                    //checks if the card is being placed
    public bool CardSelected = false;                                   //checks if the card is selected

    [Header("Grid")]
    [SerializeField] private GameObject gridParent;                     //parent object for the grid
    [SerializeField] private GameObject[] playerGrid;                   //player grid

    [Header("Battling")]
    [SerializeField] private List<GameObject> playersHand;              //players hand - used to keep track of the cards in the players hand
    public GameObject CurrentFocussedCard;                              //reference to current focussed card
    public bool FocussingACard;                                         //is the player currently focussing a card
    public int playerHealth, opponentHealth;                            //health of the player and opponent
    private readonly int maxPlayerHealth = 10, maxOpponentHealth = 10;  //max health of the player and opponent
    private int attackTurn = 0;                                         //turn count for attacking

    private void Awake()
    {
        //singleton
        instance = this;

        //gets components
        warAI = GetComponent<WarAI>();
        checkForCardsOnField = GetComponent<CheckForCardsOnField>();

        //sets health
        playerHealth = maxPlayerHealth;
        opponentHealth = maxOpponentHealth;
    }

    private void Update()
    {
        //debug function to clear playerprefs
        if (Input.GetKeyDown(KeyCode.Q))
            ClearPlayerPrefs();

        if (CurrentSelectedCard == null)
            CardSelected = false;
    }

    /// <summary>
    /// function to be called from a button - starts the dice throw
    /// </summary>
    public void StartDiceThrow()
    {
        gridParent.SetActive(true);
        StartCoroutine(ThrowDice());
    }

    IEnumerator ThrowDice()
    {
        turnCount++;

        //get random dice roll to spawn cards
        diceRoll = Random.Range(3, 6);

        //------------------------------------------------------------------- player dice logic -------------------------------------------------------------------\\

        if (isPlayerTurn)
        {
            //loop trough the dice roll and give cards to the player
            for (int i = 0; i < diceRoll; i++)
                playersHand.Add(Instantiate(card, new Vector3(cardSpawnPos[i].position.x, cardSpawnPos[i].position.y - .2f, cardSpawnPos[i].position.z), cardSpawnPos[i].rotation));

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //updates throw dice button
            UIManager.instance.DisableThrowDiceButton();
        }
        else if (!isPlayerTurn)  //-------------------------------------------- opponent dice logic ---------------------------------------------\\
        {
            //loop trough the dice roll and give cards to the opponent
            for (int i = 0; i < diceRoll; i++)
                warAI.opponentsHand.Add(Instantiate(opponentCard, warAI.opponentHandSpawnPos[i].position, warAI.opponentHandSpawnPos[i].rotation));

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //tutorial to show player what the rows do
            StartCoroutine(UIManager.instance.WarTutorialRows());

            //sets turn button to true if tutorial was a previous game
            if (PlayerPrefs.GetInt("WarTutorialRows") >= 1)
                UIManager.instance.TurnButton(true);
        }

        yield return new WaitForSeconds(3);

        isPlayerTurn = false;

        if (turnCount == 2)
            yield return null;
        else
            StartCoroutine(ThrowDice());
    }

    #region Battle System

    public void StartTurn()
    {
        StartCoroutine(TurnSystem());
    }

    IEnumerator TurnSystem()
    {

        UIManager.instance.TurnButton(false);

        //starts card placement
        StartCoroutine(warAI.AICardPlacement());

        //show tutorial about focussing cards
        StartCoroutine(UIManager.instance.FocusTutorial());

        //checks players card for later calculations
        checkForCardsOnField.CheckForPlayer();


        StartCoroutine(Attack());

        if (PlayerPrefs.GetInt("FocusTutorial") <= 0)
        {
            yield return new WaitForSeconds(7.5f);
        }
        else
        {
            yield return new WaitForSeconds(1.75f);
        }

        isPlayerTurn = true;


        if (isPlayerTurn)
        {
            UIManager.instance.TurnButton(true);
            checkForCardsOnField.CheckForAI();
        }
    }

    IEnumerator Attack()
    {
        //------------------------------------------------------------------- player battle logic -------------------------------------------------------------------\\

        attackTurn++;

        //calculate power
        int playerAttackPower = checkForCardsOnField.AttackingCount;
        int opponentAttackPower = checkForCardsOnField.AIAttackingCount;

        //if a card is selected attack selected card
        if (CurrentFocussedCard != null)
        {
            CurrentFocussedCard.GetComponent<OpponentCard>().health -= playerAttackPower;
            CurrentFocussedCard.GetComponent<OpponentCard>().UpdateCardUI();
        }

        yield return new WaitForSeconds(1.25f);

        //------------------------------------------------------------------- enemy battle logic -------------------------------------------------------------------\\

        checkForCardsOnField.CheckForAI();
        if (checkForCardsOnField.AIAttackingCount >= 1)
        {
            //get random card from players hand
            int randomPlayerCard = Random.Range(0, playersHand.Count);
            
            //subract opponents attacking power from card and update card UI
            playersHand[randomPlayerCard].GetComponent<PlayerCard>().health -= opponentAttackPower;
            playersHand[randomPlayerCard].GetComponent<PlayerCard>().UpdateCardUI();

            if (playersHand[randomPlayerCard].GetComponent<PlayerCard>().health <= 0)
                playersHand.RemoveAt(randomPlayerCard);
        }

        //wait for cards to be destroyed
        yield return new WaitForSeconds(1.25f);

        //check if there are still cards left on the field
        if (attackTurn >= 2)
            StartCoroutine(checkForCardsOnField.WarWinCheck());
    }

    #endregion

    /// <summary>
    /// function that places a selected card on a tile in the grid
    /// </summary>
    /// <param name="pos"></param>
    public void PlacePlayerCard(Vector3 pos)
    {
        if(pos != null)
        {
            PlacingCard = true;
            CurrentSelectedCard.transform.position = pos;
            CurrentSelectedCard = null;
        }
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}