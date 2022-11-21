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

    [Header("Battling")]
    private int maxPlayerHealth = 10, maxOpponentHealth = 10;
    [SerializeField] private int playerHealth, opponentHealth;

    [Header("AI")]
    [SerializeField] private Transform[] opponentHandSpawnPos;
    [SerializeField] private List<GameObject> opponentsHand;
    [SerializeField] private GameObject[] enemyGrid;
    [SerializeField] private GameObject[] enemyDefendingRow, enemyAttackingRow;
    [SerializeField] private GameObject[] playerDefendingRow, playerAttackingRow;
    [SerializeField] private int amountOnEnemyDefendingRow, amountOnEnemyAttackingRow;
    [SerializeField] private int amountOnPlayerDefendingRow, amountOnPlayerAttackingRow;
    private void Awake()
    {
        instance = this;
        //grid = FindObjectOfType<WarGrid>();

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
        }

        if (playerHealth <= 0)
        {
            playerHealth = 0;

            //AI won
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack(true, 1);
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

    public void SwitchTurn()
    {
        UIManager.instance.TurnButton(false);

        //logic for enemy turn
        StartCoroutine(AICardPlacement());
        CheckForCardsOnField();
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
                opponentsHand.Add(Instantiate(opponentCard, opponentHandSpawnPos[i].position, opponentHandSpawnPos[i].rotation));

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

    private void Attack(bool playerAttack, int amount)
    {
        if (playerAttack)
            opponentHealth -= amount;
        else
            playerHealth -= amount;
    }

    IEnumerator AICardPlacement()
    {
        for (int i = 0; i < opponentsHand.Count; i++)
        {
            int randomTile = Random.Range(0, enemyGrid.Length);
            print(randomTile);

            yield return new WaitForSeconds(.2f);

            //check if tile is empty
            if (!enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard)
            {
                //places cards on a random tile on the enemy grid
                opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, .1f, enemyGrid[randomTile].transform.position.z);
            }
            else
            {
                //if the tile has a card on it, it will check the next tile
                randomTile++;

                opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, .1f, enemyGrid[randomTile].transform.position.z);
            }
        }
    }

    private void CheckForCardsOnField()
    {
        #region PlayerCheck
        for (int i = 0; i < playerAttackingRow.Length; i++)
        {
            if (playerAttackingRow[i].GetComponent<WarTile>().HasCard)
                amountOnPlayerAttackingRow++;
        }

        for (int i = 0; i < playerDefendingRow.Length; i++)
        {
            if (playerDefendingRow[i].GetComponent<WarTile>().HasCard)
                amountOnPlayerDefendingRow++;
        }
        #endregion

        #region AiCheck
        for (int i = 0; i < enemyAttackingRow.Length; i++)
        {
            if (enemyAttackingRow[i].GetComponent<OpponentWarTile>().HasCard)
                amountOnEnemyAttackingRow++;
        }

        for (int i = 0; i < enemyDefendingRow.Length; i++)
        {
            if (enemyDefendingRow[i].GetComponent<OpponentWarTile>().HasCard)
                amountOnEnemyDefendingRow++;
        }
        #endregion
    }
}



