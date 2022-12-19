using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentWarTile : MonoBehaviour
{
    public bool HasCard = false;

    [SerializeField] private GameObject CardOn;

    private void Update()
    {
        //get card info
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        if (Physics.Raycast(ray, 0.25f))
        {
            if (!HasCard)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    WarManager.instance.enemyCardsInField.Add(hit.transform.gameObject);
                    CardOn = hit.transform.gameObject;
                }
            }

            HasCard = true;
        }
        else
        {
            HasCard = false;
            if (CardOn != null)
            {
                WarManager.instance.enemyCardsInField.Remove(CardOn);
                CardOn = null;
            }
        }
    }
}
