using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCard : Card
{
    [Header("Card Movement")]
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float speed = 0;

    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardInField;
    public WarTile posIn;
    [Header("Card Follow")]
    [SerializeField] private float offset;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private LayerMask layerToHit;

    [SerializeField] private Transform objectToAttack;

    [SerializeField] private float duration, heightY;


    private void Start()
    {
        anim = GetComponent<Animator>();
        //sets cards starting position
        startPos = transform.position;
        //gets random health value
        health = Random.Range(2, 6);
        //gets random attack value
        attack = Random.Range(1, 3);
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }
    private void Update()
    {
        UpdateCardUI();

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
        /* if (Input.GetMouseButtonDown(1) && WarManager.instance.CardSelected)
             StartCoroutine(ResetCardPosition(true));*/
        if (WarManager.instance.PlacingCard)
            StartCoroutine(ResetCardPosition(false));
        if (posIn != null)
            anim.SetBool("CardSelected", false);
        else if (posIn == null && WarManager.instance.CurrentSelectedCard == this.gameObject)
            anim.SetBool("CardSelected", true);
    }

    private void OnMouseDown()
    {
        /* if (posIn != null)
         {
             posIn = null;
         }*/

        //If card is in field and clicked on destroy card
        if (WarManager.instance.PlayerCardsInField.Contains(gameObject))
        {
            health = 0;
        }
        else if (!WarManager.instance.CardSelected)
        {
            WarManager.instance.CardSelected = true;
            //set reference to current selected card
            WarManager.instance.CurrentSelectedCard = gameObject;
            //set animation states
            anim.SetBool("CardSelected", true);
            //move up the card to indicate it being selected
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }

        
        
        /*else if (WarManager.instance.CurrentSelectedCard == this.gameObject)
        {
            WarManager.instance.CardSelected = false;
            //set reference to current selected card
            WarManager.instance.CurrentSelectedCard = null;
            //set animation states
            anim.SetBool("CardSelected", false);
            //move up the card to indicate it being selected
            transform.position = startPos;
        }*/
    }

    public void UpdateText()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    IEnumerator ResetCardPosition(bool resetCardPos)
    {
        WarManager.instance.CardSelected = false;
        //set animation state
        anim.SetBool("CardSelected", false);
        //sets card back to default position
        if (resetCardPos)
            transform.position = startPos;

        yield return new WaitForSeconds(.001f);

        WarManager.instance.CurrentSelectedCard = null;
    }

    /// <summary>
    /// Attack forward to card opposite of this gameobject
    /// </summary>
    public void AttackForward()
    {
        if (Physics.SphereCast(transform.localPosition, 0.1f, transform.up, out RaycastHit hit, 3, layerToHit))
        {
            objectToAttack = hit.transform;
            StartCoroutine(Curve(transform.position, hit.transform.position, false, hit));
        }
        else if (!Physics.SphereCast(transform.localPosition, 0.1f, transform.up, out hit, 3, layerToHit))
        {
            WarManager.instance.opponentHealth -= attack;
            StartCoroutine(Curve(transform.position, new Vector3(0.4f, 7.5f, -8.2f), false, hit));
        }
    }

    /// <summary>
    /// update health (and attack if needs to)
    /// </summary>
    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    public IEnumerator Curve(Vector3 start, Vector3 target, bool on, RaycastHit damage)
    {
        bool used = on;
        Vector3 nowPos = transform.position;
        float timePassed = 0;
        Vector3 end = target;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;

            float linearT = timePassed / duration;
            float heightT = movementCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0, heightY, heightT);

            transform.position = Vector3.Lerp(start, end, linearT) + new Vector3(0, height, 0);

            if (used == false)
            {
                if (transform.position == end)
                {
                    if (damage.transform != null)
                        damage.transform.gameObject.GetComponent<OpponentCard>().health -= attack;

                    VirtualCameraSettings.instance.Hit(0.1f);
                    StartCoroutine(Curve(transform.position, nowPos, true, damage));
                    used = true;
                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.TransformDirection(Vector3.up) * 3f;
        Debug.DrawRay(transform.position, forward, Color.red);
    }
}