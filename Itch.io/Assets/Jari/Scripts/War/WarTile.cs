using System.Collections;
using UnityEngine;

public class WarTile : MonoBehaviour
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

    IEnumerator ResetPlacingCard()
    {
        yield return new WaitForSeconds(0.2f);
        WarManager.instance.PlacingCard = false;
    }
}
