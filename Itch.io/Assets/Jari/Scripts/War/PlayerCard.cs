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

    private bool goAttack = false;

    [SerializeField] private Transform objectToAttack;

    [SerializeField] private float duration, heightY;


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
        if (posIn != null)
        {
            posIn = null;
        }

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
        else if (WarManager.instance.CurrentSelectedCard == this.gameObject)
        {
            WarManager.instance.CardSelected = false;

            //set reference to current selected card
            WarManager.instance.CurrentSelectedCard = null;

            //set animation states
            anim.SetBool("CardSelected", false);

            //move up the card to indicate it being selected
            transform.position = startPos;
        }
    }

<<<<<<< Updated upstream
    public IEnumerator ResetCardPosition(bool resetPos)
=======
    public void UpdateText()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    IEnumerator ResetCardPosition(bool resetCardPos, bool placeCheck)
>>>>>>> Stashed changes
    {
        WarManager.instance.CardSelected = false;
        print("reset" + this.gameObject.name);
        //set animation state
        anim.SetBool("CardSelected", false);

        //sets card back to default position
        if (resetPos)
            transform.position = startPos;

        yield return new WaitForSeconds(.001f);

        WarManager.instance.CurrentSelectedCard = null;
    }

    /// <summary>
    /// Attack forward to card opposite of this gameobject
    /// </summary>
    public void AttackForward()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.localPosition, 0.1f, transform.up, out hit, 3, layerToHit))
        {
            hit.transform.gameObject.GetComponent<OpponentCard>().health -= attack;
            print(hit.transform.gameObject.name);
            objectToAttack = hit.transform;
            StartCoroutine(Curve(transform.position, hit.transform.position, false));
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

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.TransformDirection(Vector3.up) * 3f;
        Debug.DrawRay(transform.position, forward, Color.red);
    }

    public IEnumerator Curve(Vector3 start, Vector3 target, bool on)
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
                    VirtualCameraSettings.instance.Hit(0.1f);
                    StartCoroutine(Curve(transform.position, nowPos, true));
                    used = true;
                }
            }

            yield return null;
        }
    }
}
