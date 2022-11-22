using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCard : MonoBehaviour
{
    [Header("Card Info")]
    public int attack = 1;
    public int health = 2;

    /*private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (!WarManager.instance.FocussingACard)
        {
            WarManager.instance.FocussingACard = true;
            WarManager.instance.CurrentFocussedCard = gameObject;
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }*/
}
