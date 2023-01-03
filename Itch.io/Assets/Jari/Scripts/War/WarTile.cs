using System.Collections;
using UnityEngine;

public class WarTile : MonoBehaviour
{
    public bool HasCard = false;
    [SerializeField] private GameObject CardOn;

    private Vector3 startPos;

    private void Update()
    {
        //get card info
        Ray ray = new(transform.position, transform.up);
        if (Physics.Raycast(ray, 0.25f))
        {
            if (!HasCard)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    /*for (int i = WarManager.instance.playersHand.IndexOf(hit.transform.gameObject); i > WarManager.instance.playersHand.Count; i++)
                    {
                        WarManager.instance.placeToSpawn = i;
                    }*/
                    
                    WarManager.instance.playersHand.Remove(hit.transform.gameObject);
                    WarManager.instance.PlayerCardsInField.Add(hit.transform.gameObject);
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
                WarManager.instance.PlayerCardsInField.Remove(CardOn);
                CardOn = null;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!HasCard)
        {
            //places card on tile if card is selected
            if (WarManager.instance.CardSelected)
            {
                WarManager.instance.PlacePlayerCard(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z));
                StartCoroutine(ResetPlacingCard());
            }
        }
    }

    private void OnMouseEnter()
    {
        if (WarManager.instance.CurrentSelectedCard != null && !HasCard)
        {
            startPos = WarManager.instance.CurrentSelectedCard.transform.position;

            WarManager.instance.CurrentSelectedCard.transform.position = new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z);
            WarManager.instance.CurrentSelectedCard.GetComponent<BoxCollider>().enabled = false; 
        }
    }

    /*private void OnMouseExit()
    {
        if (WarManager.instance.CurrentSelectedCard != null)
        {
            WarManager.instance.CurrentSelectedCard.transform.position = startPos;
        }
    }*/

    IEnumerator ResetPlacingCard()
    {
        yield return new WaitForSeconds(0.2f);
        WarManager.instance.PlacingCard = false;
    }
}
