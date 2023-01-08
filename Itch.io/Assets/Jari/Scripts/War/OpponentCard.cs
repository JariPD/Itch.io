using System.Collections;
using TMPro;
using UnityEngine;

public class OpponentCard : Card
{
    [SerializeField] private AnimationCurve movementCurve;
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
        health = Random.Range(1, 7);
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
            StartCoroutine(Disolve()); //starts disolving the card
    }

    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    /// <summary>
    /// Attack forward to card opposite of this gameobject if no card hit player
    /// </summary>
    public void AttackForward()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 3, layerToHit))
        {
            objectToAttack = hit.transform;
            StartCoroutine(Curve(transform.position, hit.transform.position, false, hit));
        }
        else if (!Physics.Raycast(transform.position, -transform.up, out hit, 3, layerToHit))
        {
            WarManager.instance.playerHealth -= attack;
            StartCoroutine(Curve(transform.position, new Vector3(0.8f, 7.5f, -20), false, hit));
        }
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
                        damage.transform.gameObject.GetComponent<PlayerCard>().health -= attack;

                    if (damage.transform != null && damage.transform.gameObject.GetComponent<PlayerCard>().health != 0)
                        WarManager.instance.audioManager.Play("CardHit");
                    else if (Vector3.Distance(transform.position, new Vector3(0.8f, 7.5f, -20)) <= 0.1f)
                        WarManager.instance.audioManager.Play("CardHit");
                    
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
        Vector3 forward = -transform.TransformDirection(Vector3.up) * 3;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
