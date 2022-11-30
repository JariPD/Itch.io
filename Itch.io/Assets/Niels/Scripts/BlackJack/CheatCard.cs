using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatCard : MonoBehaviour
{
    [SerializeField] public int cheatCardValue { get; private set; }

    [Header("Player Info")]
    [SerializeField] private CheatCardDeck cheatCardDeck;

    public bool UseAble = false;

    [Header("Card Info")]
    public int cardNumber = 0;
    public int ownNumber;

    private void Awake()
    {
        ChooseValue();
    }

    private void Start()
    {
        cheatCardDeck = FindObjectOfType<CheatCardDeck>();
        GetComponentInChildren<TextMeshProUGUI>().text = cheatCardValue.ToString();
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, cheatCardDeck.cardPos[ownNumber].transform.position, 0.1f * Time.maximumDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, cheatCardDeck.cardPos[ownNumber].transform.rotation, 0.1f * Time.maximumDeltaTime);
    }

    private void ChooseValue()
    {
        cheatCardValue = Random.Range(-2, 3);
        if (cheatCardValue == 0)
            ChooseValue();
    }

    /// <summary>
    /// for button to use this cheat card
    /// </summary>
    public void UsingCard()
    {
        BlackJackManager.instance.UseCheatCard(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (UseAble)
        {
            BlackJackManager.instance.UseCheatCard(this.gameObject);
            UseAble = false;
        }
    }
}
