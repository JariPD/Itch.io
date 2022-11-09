using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackCard : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private PlayerDeck playerDeck;

    [Header("Card Info")]
    public int cardNumber = 0;
    public int ownNumber;

    private void Start()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();
        GetComponentInChildren<TextMeshProUGUI>().text = cardNumber.ToString();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerDeck.cardPos[ownNumber].transform.position, 1 * Time.maximumDeltaTime);        
    }
}
