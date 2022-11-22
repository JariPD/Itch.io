using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCard : MonoBehaviour
{

    private void Update()
    {
        if (WarManager.instance.FocussingACard)
        {
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }

    private void OnMouseDown()
    {
        WarManager.instance.FocussingACard = true;
        
    }
}
