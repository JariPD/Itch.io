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

    private void Awake()
    {
        instance = this;
    }

    public void StartDiceThrow()
    {
        StartCoroutine(ThrowDice());
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
            UIManager.instance.UpdateDiceRollText(diceRoll);

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
            UIManager.instance.UpdateOpponentDiceRollText(diceRoll);
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
