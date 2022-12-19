using System.Collections;
using UnityEngine;

public class WarTile : MonoBehaviour
{
    public bool HasCard = false;
<<<<<<< Updated upstream
    public GameObject card;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 0.25f))
        {
            card = hit.transform.gameObject;
            card.GetComponent<PlayerCard>().posIn = this;
=======
    [SerializeField] private GameObject CardOn;

    private void Update()
    {
        //get info about card
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        if (Physics.Raycast(ray, 0.25f))
        {
            if (!HasCard)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    WarManager.instance.PlayerCardsInField.Add(hit.transform.gameObject);
                    CardOn = hit.transform.gameObject;
                }
            }

>>>>>>> Stashed changes
            HasCard = true;
        }
        else
        {
            HasCard = false;
<<<<<<< Updated upstream

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
=======
            if (CardOn != null)
            {
                WarManager.instance.PlayerCardsInField.Remove(CardOn);
                CardOn = null;
            }
        }
>>>>>>> Stashed changes
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
