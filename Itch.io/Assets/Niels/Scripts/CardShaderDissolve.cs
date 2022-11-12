using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardShaderDissolve : MonoBehaviour
{
    private Animator cardDissolveAnim;

    private void Start()
    {
        cardDissolveAnim = GetComponent<Animator>();
    }

    public void DissolveCard()
    {
        cardDissolveAnim.Play("CardDissolve");
    }
}
