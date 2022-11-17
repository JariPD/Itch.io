using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarManager : MonoBehaviour
{
    public static WarManager instance;

    public int diceRoll;
    private bool isPlayerTurn = true;
    private int turnCount = 0;

    [SerializeField] private List<GameObject> usersCards;
    [SerializeField] private GameObject card, opponentCard;
    [SerializeField] private Transform[] cardSpawnPos;

    //[SerializeField] private GameObject dice;
    //[SerializeField] private DiceThrow diceThrow;

    [Header("Card Placement")]
    public GameObject CurrentSelectedCard;
    public bool PlacingCard = false;
    public bool CardSelected;

    [Header("Grid")]
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject[] playerGrid;
    //private WarGrid grid;

    [Header("AI")]
    [SerializeField] private Transform[] opponentCardSpawnPos;
    [SerializeField] private List<GameObject> opponentsCards;
    [SerializeField] private GameObject[] enemyDefendingRow, enemyAttackingRow;
    [SerializeField] private bool canPlaceOnDefendingRow;
    [SerializeField] private bool canPlaceOnAttackingRow;
    [SerializeField] private int amountOnDefendingRow = 0;
    [SerializeField] private int amountOnAttackingRow = 0;

    private void Awake()
    {
        instance = this;
        //grid = FindObjectOfType<WarGrid>();
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
    public void PlaceCard(Vector3 pos)
    {
        PlacingCard = true;
        CurrentSelectedCard.transform.position = pos;
    }

    public void Turn()
    {
        UIManager.instance.TurnButton(false);

        //logic for enemy turn
        AICardPlacement();
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
            {
                usersCards.Add(card);
                Instantiate(card, cardSpawnPos[i].position, cardSpawnPos[i].rotation);
            }

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //updates throw dice button
            UIManager.instance.DisableThrowDiceButton();
        }
        else
        {
            for (int i = 0; i < diceRoll; i++)
            {
                opponentsCards.Add(card);
                Instantiate(opponentCard, opponentCardSpawnPos[i].position, opponentCardSpawnPos[i].rotation);
            }

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

    public void AICardPlacement()
    {

        

        
        //code for second turn - not working yet
        /*for (int i = 0; i < enemyDefendingRow.Length; i++)
        {
            //check how many cards are on defending row
            if (enemyDefendingRow[i].GetComponent<WarTile>().hasCard)
                amountOnDefendingRow++;

            //max defending cards check
            if (enemyDefendingRow[i].GetComponent<WarTile>().hasCard)
                canPlaceOnDefendingRow = false;
        }

        for (int i = 0; i < enemyAttackingRow.Length; i++)
        {
            //check how many cards are on attacking row
            if (enemyAttackingRow[i].GetComponent<WarTile>().hasCard)
                amountOnAttackingRow++;

            //max attacking cards check
            if (enemyAttackingRow[i].GetComponent<WarTile>().hasCard)
            {
                //cant place on attacking row
                canPlaceOnAttackingRow = false;
            }
        }*/

        //check AI's grid
        //check how many are on defending row
        //check how many are on attacking row

        //if defending row is full, place on attacking row

        //if attacking row is full, place on defending row

        //place cards accordingly
    }
}

