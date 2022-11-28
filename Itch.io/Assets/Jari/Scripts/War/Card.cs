using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    protected Animator anim;

    [Header("Card Info")]
    public int attack = 1;
    public int health = 2;

    protected IEnumerator Disolve()
    {
        WarManager.instance.CurrentFocussedCard = null;
        WarManager.instance.FocussingACard = false;
        anim.SetTrigger("Disolve");

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
