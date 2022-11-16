using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WarCard : MonoBehaviour
{
    [Header("References")]
    private Animator anim;

    [Header("Card Info")]
    private int attack = 2;
    private int health = 3;

    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardSelected;

    [Header("Card Follow")]
    [SerializeField] private float offset;
    private Vector3 pos;

    private float t;


    private void Start()
    {
        anim = GetComponent<Animator>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (cardSelected)
        {
            //card hover
            /*pos = Input.mousePosition;
            pos.z = offset;
            //transform.position = Camera.main.ScreenToWorldPoint(pos);
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, 0.75f, Camera.main.ScreenToWorldPoint(pos).y);*/
        }

        if (Input.GetMouseButtonDown(1) && cardSelected)
            ResetCardPosition(true);

        if (WarManager.instance.PlacingCard)
        {
            ResetCardPosition(false);
        }
    }

    private void OnMouseDown()
    {
        if (!cardSelected)
        {
            cardSelected = true;

            //set reference to current selected card
            WarManager.instance.CurrentSelectedCard = gameObject;

            //set animation states
            anim.SetBool("CardSelected", true);

            //move up the card to indicate it being selected
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }

    private void ResetCardPosition(bool resetPos)
    {
        cardSelected = false;

        //set animation state
        anim.SetBool("CardSelected", false);

        //sets card back to default position
        if (resetPos)
            transform.position = startPos;

        //clear selected card
        //WarManager.instance.CurrentSelectedCard = null;
    }
}
