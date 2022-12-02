using TMPro;
using UnityEngine;

public class OpponentCard : Card
{
    [SerializeField] private GameObject outline;
    private WarAI ai;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

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

            WarManager.instance.CurrentFocussedCard = gameObject;
            outline.SetActive(true);
        }
    }
    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }
}
