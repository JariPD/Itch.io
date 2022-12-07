using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    protected Animator anim;

    [Header("Card Info")]
    public int attack = 1;
    public int health = 2;

    /// <summary>
    /// IEnumerator that disolves the card
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Disolve()
    {
        //clear focused card reference and set focused bool to false
        WarManager.instance.CurrentFocussedCard = null;
        WarManager.instance.FocussingACard = false;

        //set animation states
        anim.SetBool("CardSelected", false);
        anim.SetTrigger("Disolve");

        yield return new WaitForSeconds(1f);

        //wait for disolve shader to finish and turn off gameobject
        gameObject.SetActive(false);
    }
}
