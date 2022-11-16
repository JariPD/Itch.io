using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarManager : MonoBehaviour
{
    public static WarManager instance;

    public int diceRoll;
    private bool isPlayerTurn = true;
    private int turnCount = 0;

    [Header("Card Lists")]
    [SerializeField] private List<GameObject> usersCards;
    [SerializeField] private List<GameObject> opponentsCards;

    [SerializeField] private GameObject card;
    [SerializeField] private Transform[] cardSpawnPos;
    [SerializeField] private Transform[] opponentCardSpawnPos;
    //[SerializeField] private GameObject dice;
    //[SerializeField] private DiceThrow diceThrow;

    [Header("Card Placement")]
    public GameObject CurrentSelectedCard;
    public bool PlacingCard = false;

    [Header("Grid")]
    [SerializeField] private GameObject gridParent;
    private WarGrid grid;
    

    private void Awake()
    {
        instance = this;

        grid = FindObjectOfType<WarGrid>();
    }

    public void StartDiceThrow()
    {
        //grid.CreateGrid();
        gridParent.SetActive(true);
        StartCoroutine(ThrowDice());
    }

    public void PlaceCard(Vector3 pos)
    {
        PlacingCard = true;
        
        CurrentSelectedCard.transform.position = pos;
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
                Instantiate(card, opponentCardSpawnPos[i].position, opponentCardSpawnPos[i].rotation);
            }

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);
        }

        yield return new WaitForSeconds(3);

        isPlayerTurn = false;

        if (turnCount == 2)
            yield return null;
        else
        {
            StartCoroutine(ThrowDice());
        }
    }
}
