using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    protected Animator anim;

    [Header("Card Info")]
    public int attack;
    public int health;

    /// <summary>
    /// IEnumerator that disolves the card
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Disolve()
    {
        WarManager.instance.audioManager.Play("CardDisolve");
        
        //set animation states
        anim.SetBool("CardSelected", false);
        anim.SetTrigger("Disolve");

        yield return new WaitForSeconds(1f);

        //wait for disolve shader to finish and turn off gameobject
        gameObject.SetActive(false);
    }
}
