using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarTile : MonoBehaviour
{
    public bool HasCard = false;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, 0.25f))
            HasCard = true;
        else 
            HasCard = false;
    }

    private void OnMouseDown()
    {
        if (!HasCard)
        {
            //place selected card on tile
            WarManager.instance.PlacePlayerCard(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z));

            StartCoroutine(ResetPlacingCard());
        }
    }

    IEnumerator ResetPlacingCard()
    {
        yield return new WaitForSeconds(0.2f);
        WarManager.instance.PlacingCard = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 0.25f);
    }
}
