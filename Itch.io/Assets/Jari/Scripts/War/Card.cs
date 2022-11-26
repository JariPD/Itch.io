using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    protected Animator anim;

    [Header("Card Info")]
    public int attack = 1;
    public int health = 3;

    protected TextMeshProUGUI attackText;
    protected TextMeshProUGUI healthText;

    private void Update()
    {
        if (health <= 0)
            StartCoroutine(Disolve());
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
