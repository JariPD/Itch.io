using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarManager : MonoBehaviour
{
    public static WarManager instance;

    public int diceRoll;
    private bool isPlayerTurn = true;
    private int turnCount = 0;

    [Header("References")]
    [SerializeField] private GameObject card, opponentCard;
    [SerializeField] private Transform[] cardSpawnPos;
    private WarAI warAI;
    private CheckForCardsOnField checkForCardsOnField;

    //[SerializeField] private GameObject dice;
    //[SerializeField] private DiceThrow diceThrow;

    [Header("Card Placement")]
    public GameObject CurrentSelectedCard;
    public bool PlacingCard = false;
    public bool CardSelected;

    [Header("Grid")]
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject[] playerGrid;

    [Header("Battling")]
    public bool FocussingACard;
    private int maxPlayerHealth = 10, maxOpponentHealth = 10;
    [SerializeField] private int playerHealth, opponentHealth;



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
        //healthchecks
        if (opponentHealth <= 0)
        {
            opponentHealth = 0;

            //player won
            print("player won");
        }

        if (playerHealth <= 0)
        {
            playerHealth = 0;

            //AI won
        }
    }

    /// <summary>
    /// function to be called from a button - starts the dice throw
    /// </summary>
    public void StartDiceThrow()
    {
        //grid.CreateGrid();
        gridParent.SetActive(true);
        StartCoroutine(ThrowDice());
    }

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

    public void StartTurn()
    {
        StartCoroutine(TurnSystem());
    }

    IEnumerator TurnSystem()
    {
        UIManager.instance.TurnButton(false);

        //logic for enemy turn
        //starts card placement
        StartCoroutine(warAI.AICardPlacement());

        //checks players card for later calculations
        checkForCardsOnField.CheckForPlayer();

        yield return new WaitForSeconds(1f);

        isPlayerTurn = true;

        if (isPlayerTurn)
        {
            UIManager.instance.TurnButton(true);
            checkForCardsOnField.CheckForAI();
        }
    }

    IEnumerator ThrowDice()
    {
        //Instantiate(dice, new Vector3(0, 5, 0), Quaternion.identity);
        //diceThrow = FindObjectOfType<DiceThrow>();
        //diceThrow.Throw();

        turnCount++;

        diceRoll = Random.Range(1, 6);

        if (isPlayerTurn)
        {
            for (int i = 0; i < diceRoll; i++)
                Instantiate(card, cardSpawnPos[i].position, cardSpawnPos[i].rotation);

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

    private void Attack()
    {
        //player power
        int playerAttackPower = checkForCardsOnField.AttackingCount;
        int playerDefendingPower = checkForCardsOnField.DefendingCount * 2;

        //AI power
        int opponentAttackPower = checkForCardsOnField.AIAttackingCount;
        int opponentDefendingPower = checkForCardsOnField.AIDefendingCount * 2;

        if (playerAttackPower > opponentDefendingPower)
        {
            
        }
    }
}