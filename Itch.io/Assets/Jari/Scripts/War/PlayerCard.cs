using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCard : Card
{
    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardInField;

    [Header("Card Follow")]
    [SerializeField] private float offset;


    private void Start()
    {
        anim = GetComponent<Animator>();
        startPos = transform.position;

        attackText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        healthText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        //sets text to the correct value
        attackText.text = attack.ToString();
        healthText.text = health.ToString();

        if (Input.GetMouseButtonDown(1) && WarManager.instance.CardSelected)
            StartCoroutine(ResetCardPosition(true));

        if (WarManager.instance.PlacingCard)
            StartCoroutine(ResetCardPosition(false));
    }

    private void OnMouseDown()
    {
        if (!WarManager.instance.CardSelected)
        {
            WarManager.instance.CardSelected = true;

            //set reference to current selected card
            WarManager.instance.CurrentSelectedCard = gameObject;

            //set animation states
            anim.SetBool("CardSelected", true);

            //move up the card to indicate it being selected
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }

    IEnumerator ResetCardPosition(bool resetPos)
    {
        WarManager.instance.CardSelected = false;

        //set animation state
        anim.SetBool("CardSelected", false);

        //sets card back to default position
        if (resetPos)
            transform.position = startPos;

        yield return new WaitForSeconds(.1f);

        WarManager.instance.CurrentSelectedCard = null;
    }
}
