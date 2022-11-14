using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WarCard : MonoBehaviour
{
    [Header("Card Info")]
    private int attack = 2;
    private int health = 3;

    [Header("Anim")]
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseOver()
    {
        anim.SetBool("Hover", true);
    }

    private void OnMouseExit()
    {
        anim.SetBool("Hover", false);
    }
}
