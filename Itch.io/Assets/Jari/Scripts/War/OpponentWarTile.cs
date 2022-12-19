using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentWarTile : MonoBehaviour
{
    public bool HasCard = false;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, 0.25f))
            HasCard = true;
        else
            HasCard = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 0.25f);
    }
}
