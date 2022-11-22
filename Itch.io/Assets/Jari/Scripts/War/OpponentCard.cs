using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCard : MonoBehaviour
{
    private Animator anim;

    [Header("Card Info")]
    public int attack = 1;
    public int health = 2;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }

    IEnumerator Disolve()
    {
        WarManager.instance.CurrentFocussedCard = null;
        WarManager.instance.FocussingACard = false;
        anim.SetTrigger("Disolve");

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        
    }
}
