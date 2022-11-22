using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCard : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (!WarManager.instance.FocussingACard)
        {
            WarManager.instance.FocussingACard = true;
            WarManager.instance.CurrentFocussedCard = gameObject;
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }
}
