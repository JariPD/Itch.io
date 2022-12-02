using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCard : Card
{
    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardInField;

    [Header("Card Follow")]
    [SerializeField] private float offset;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        anim = GetComponent<Animator>();

        //sets cards starting position
        startPos = transform.position;

        //gets random health value
        health = Random.Range(1, 4);

        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    private void Update()
    {
        if (health <= 0)
        {
            health = 0;

            //turns off text
            attackText.enabled = false;
            healthText.enabled = false;

            //starts disolving the card
            StartCoroutine(Disolve());
        }

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

    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }
}
