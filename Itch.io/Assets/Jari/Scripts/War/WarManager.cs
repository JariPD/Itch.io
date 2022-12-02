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
    [SerializeField] private Transform[] cardSpawnPos;
    [SerializeField] private GameObject card, opponentCard;
    private WarAI warAI;
    private CheckForCardsOnField checkForCardsOnField;

    [Header("Card Placement")]
    public GameObject CurrentSelectedCard;
    public bool PlacingCard = false;
    public bool CardSelected = false;

    [Header("Grid")]
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject[] playerGrid;

    [Header("Battling")]
    [SerializeField] private List<GameObject> playersHand;
    public GameObject CurrentFocussedCard;
    public bool FocussingACard;
    public int playerHealth, opponentHealth;
    private readonly int maxPlayerHealth = 10, maxOpponentHealth = 10;
    private int attackTurn = 0;

    private void Awake()
    {
        instance = this;
        warAI = GetComponent<WarAI>();
        checkForCardsOnField = GetComponent<CheckForCardsOnField>();

        playerHealth = maxPlayerHealth;
        opponentHealth = maxOpponentHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClearPlayerPrefs();
        }
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

        diceRoll = Random.Range(3, 6);

        if (isPlayerTurn)
        {
            for (int i = 0; i < diceRoll; i++)
                playersHand.Add(Instantiate(card, new Vector3(cardSpawnPos[i].position.x, cardSpawnPos[i].position.y - .2f, cardSpawnPos[i].position.z), cardSpawnPos[i].rotation));

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //updates throw dice button
            UIManager.instance.DisableThrowDiceButton();
        }
        else
        {
            //instantiates the cards for the enemy
            for (int i = 0; i < diceRoll; i++)
                warAI.opponentsHand.Add(Instantiate(opponentCard, warAI.opponentHandSpawnPos[i].position, warAI.opponentHandSpawnPos[i].rotation));

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //turns on "Next Turn" button
            UIManager.instance.TurnButton(true);
        }

        yield return new WaitForSeconds(3);

        isPlayerTurn = false;

        if (turnCount == 2)
            yield return null;
        else
            StartCoroutine(ThrowDice());
    }

    public void StartTurn()
    {
        StartCoroutine(TurnSystem());
    }

    #region Battle System

    IEnumerator TurnSystem()
    {
        UIManager.instance.TurnButton(false);

        //logic for enemy turn
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
        //enemy turn

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
        PlacingCard = true;
        CurrentSelectedCard.transform.position = pos;
        CurrentSelectedCard = null;
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}