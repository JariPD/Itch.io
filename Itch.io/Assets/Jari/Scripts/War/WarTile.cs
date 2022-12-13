using System.Collections;
using UnityEngine;

public class WarTile : MonoBehaviour
{
    public bool HasCard = false;
    public GameObject card;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 0.25f))
        {
            card = hit.transform.gameObject;
            card.GetComponent<PlayerCard>().posIn = this;
            HasCard = true;
        }
        else
            HasCard = false;

        if (HasCard == false)
            if(card != null)
            {
                card.GetComponent<PlayerCard>().posIn = null;
                if (card.GetComponent<PlayerCard>().posIn == null)
                    card = null;                   
            }

        //if (HasCard == false && WarManager.instance.CurrentSelectedCard == null)
        //{
        //    if (card != null)
        //        card.GetComponent<PlayerCard>().ResetCardPosition(true);
        //    //card = null;
        //}
    }

    private void OnMouseDown()
    {
        if (!HasCard)
        {
            //places card on tile if card is selected
            if (WarManager.instance.CardSelected)
            {
                WarManager.instance.PlacePlayerCard(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z));
                WarManager.instance.PlacingCard = false;
            }
        }
    }
}
