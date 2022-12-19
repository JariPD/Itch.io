using System.Collections;
using TMPro;
using UnityEngine;

public class OpponentCard : Card
{

    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private GameObject outline;
    private WarAI ai;
    public OpponentWarTile posIn;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private LayerMask layerToHit;

    [SerializeField] private Transform objectToAttack;

    [SerializeField] private float duration, heightY;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ai = FindObjectOfType<WarAI>();

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
            //turns off text
            attackText.enabled = false;
            healthText.enabled = false;
            
            outline.SetActive(false);

            //starts disolving the card
            StartCoroutine(Disolve());
        }

        if (WarManager.instance.CurrentFocussedCard == null)
        {
            for (int i = 0; i < ai.opponentsHand.Count; i++)
                ai.opponentsHand[i].GetComponent<OpponentCard>().outline.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!WarManager.instance.FocussingACard)
        {
            WarManager.instance.FocussingACard = true;
            WarManager.instance.CurrentFocussedCard = gameObject;
            outline.SetActive(true);
        }
        else if (WarManager.instance.FocussingACard)
        {
            for (int i = 0; i < ai.opponentsHand.Count; i++)
                ai.opponentsHand[i].GetComponent<OpponentCard>().outline.SetActive(false);
            //if (!WarManager.instance.FocussingACard)
            //{
            //    WarManager.instance.FocussingACard = true;
            //    WarManager.instance.CurrentFocussedCard = gameObject;
            //    outline.SetActive(true);
            //}
            //else if (WarManager.instance.FocussingACard)
            //{
            //    for (int i = 0; i < ai.opponentsHand.Count; i++)
            //        ai.opponentsHand[i].GetComponent<OpponentCard>().outline.SetActive(false);

            WarManager.instance.CurrentFocussedCard = gameObject;
            outline.SetActive(true);
            //    WarManager.instance.CurrentFocussedCard = gameObject;
            //    outline.SetActive(true);
            }
        }

    /// <summary>
    /// Attack forward to card opposite of this gameobject
    /// </summary>
    public void AttackForward()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.localPosition, 0.1f, transform.up, out hit, 3, layerToHit))
        {
            hit.transform.gameObject.GetComponent<PlayerCard>().health -= attack;
            print(hit.transform.gameObject.name);
            objectToAttack = hit.transform;
            StartCoroutine(Curve(transform.position, hit.transform.position, false));
        }
    }

    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
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
                    //VirtualCameraSettings.instance.Hit(0.1f);
                    StartCoroutine(Curve(transform.position, nowPos, true));
                    used = true;
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 forward = -transform.TransformDirection(Vector3.up) * 3f;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
