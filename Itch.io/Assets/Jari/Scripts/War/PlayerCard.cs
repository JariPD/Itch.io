using System.Collections;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerCard : Card
{
    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardInField;
    private float selectedTimer;

    [Header("Card Follow")]
    [SerializeField] private float offset;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();

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

            WarManager.instance.CardSelected = false;

            //starts disolving the card
            StartCoroutine(Disolve());
        }

        if (Input.GetMouseButtonDown(1) && WarManager.instance.CardSelected)
            StartCoroutine(ResetCardPosition(resetCardPos: true, placeCheck: true));

        if (WarManager.instance.PlacingCard)
            StartCoroutine(ResetCardPosition(resetCardPos: false, placeCheck: false));

        //selected fallback
        if (WarManager.instance.CardSelected)
        {
            selectedTimer += Time.deltaTime;
            if (selectedTimer >= 3)
            {
                selectedTimer = 0;
                StartCoroutine(ResetCardPosition(resetCardPos: true, placeCheck: true));
            }
        }
        else
            selectedTimer = 0;
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

    IEnumerator ResetCardPosition(bool resetCardPos, bool placeCheck)
    {
        if (WarManager.instance.CurrentSelectedCard != gameObject && placeCheck)
            yield break;

        WarManager.instance.CardSelected = false;

        //set animation state
        anim.SetBool("CardSelected", false);

        //sets card back to default position
        if (resetCardPos)
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
