using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarTile : MonoBehaviour
{
    [SerializeField] private bool hasCard = false;
    private bool checkForCard;

    private void Update()
    {
        if (checkForCard)
        {
            hasCard = Physics.Raycast(transform.position, transform.up, 2f);
            StartCoroutine(CheckForCard());
        }
    }

    private void OnMouseDown()
    {
        if (!hasCard)
        {
            //place selected card on tile
            WarManager.instance.PlaceCard(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z));

            StartCoroutine(ResetPlacingCard());

            checkForCard = true;
        }
    }

    IEnumerator ResetPlacingCard()
    {
        yield return new WaitForSeconds(0.2f);
        WarManager.instance.PlacingCard = false;
    }

    IEnumerator CheckForCard()
    {
        yield return new WaitForSeconds(0.2f);
        checkForCard = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 2f);
    }
}
