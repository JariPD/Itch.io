using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarManager : MonoBehaviour
{
    public int diceRoll;
    private bool isPlayerTurn = true;

    [Header("Card Lists")]
    [SerializeField] private List<GameObject> usersCards;
    [SerializeField] private List<GameObject> opponentsCards;

    [SerializeField] private GameObject card;

    public void StartDiceThrow()
    {
        StartCoroutine(ThrowDice());
    }

    IEnumerator ThrowDice()
    {
        diceRoll = Random.Range(1, 6);

        if (isPlayerTurn)
        {
            for (int i = 0; i < diceRoll; i++)
                usersCards.Add(card);

            UIManager.instance.UpdateDiceRollText(diceRoll);
        }
        else
        {
            for (int i = 0; i < diceRoll; i++)
                opponentsCards.Add(card);

            UIManager.instance.UpdateOpponentDiceRollText(diceRoll);
        }

        yield return new WaitForSeconds(3);

        isPlayerTurn = false;
        ThrowDice();
    }
}
