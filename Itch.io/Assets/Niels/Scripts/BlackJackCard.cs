using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackCard : MonoBehaviour
{
    [Header("Card Info")]
    public int cardNumber;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = cardNumber.ToString();
    }
}
