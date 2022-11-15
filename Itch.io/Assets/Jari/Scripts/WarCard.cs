using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WarCard : MonoBehaviour
{
    [Header("Card Info")]
    private int attack = 2;
    private int health = 3;

    [Header("Card Settings")]
    private Vector3 startPos;
    [SerializeField] private bool cardSelected;
    private bool validSpot = false;

    [Header("Card Follow")]
    [SerializeField] private float offset;
    private Vector3 pos;

    private float t;


    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (cardSelected)
        {
            pos = Input.mousePosition;
            pos.z = offset;
            transform.position = Camera.main.ScreenToWorldPoint(pos);
            //transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(pos).y, transform.position.z);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) && !validSpot && cardSelected)
        {
            t++;
            if (t >= 2)
            {
                t = 0;
                cardSelected = false;
                transform.position = startPos;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!cardSelected)
        {
            cardSelected = true;
        }
    }
}
