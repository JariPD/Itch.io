using UnityEngine;

public class CardCheck : MonoBehaviour
{
    public bool HasCard;
    [SerializeField] private LayerMask cardLayer;

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 0.25f);
    }
    private void Update()
    {
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 0.25f, cardLayer))
        {
            HasCard = true;
        }
        else HasCard = false;
    }
}

