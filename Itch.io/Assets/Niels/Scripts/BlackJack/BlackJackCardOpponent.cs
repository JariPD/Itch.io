using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackCardOpponent : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private BlackJackOpponent opponentDeck;

    [Header("Card Info")]
    public int cardNumber = 0;
    public int ownNumber;

    private void Start()
    {
        opponentDeck = FindObjectOfType<BlackJackOpponent>();
        GetComponentInChildren<TextMeshProUGUI>().text = cardNumber.ToString();
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, opponentDeck.cardPos[ownNumber].transform.position, 0.1f * Time.maximumDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, opponentDeck.cardPos[ownNumber].transform.rotation, 0.1f * Time.maximumDeltaTime);
    }
}
