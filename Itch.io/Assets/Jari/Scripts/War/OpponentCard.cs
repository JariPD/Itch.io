using TMPro;
using UnityEngine;

public class OpponentCard : Card
{
    [SerializeField] private GameObject outline;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //gets random health value
        health = Random.Range(1, 4);

        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    private void Update()
    {
        if (health <= 0)
            StartCoroutine(Disolve());
    }

    private void OnMouseDown()
    {
        if (!WarManager.instance.FocussingACard)
        {
            WarManager.instance.FocussingACard = true;
            WarManager.instance.CurrentFocussedCard = gameObject;
            //outline.SetActive(true);
        }
    }
    public void UpdateCardUI()
    {
        //update text
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }
}
