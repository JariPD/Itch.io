using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glow : MonoBehaviour
{
    [SerializeField] private GameObject glow;
    [SerializeField] private Button interactable;

    private void Update()
    {
        if (interactable.interactable == false)
            glow.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (interactable.interactable == true)
            glow.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (interactable.interactable == true)
            glow.SetActive(false);
    }
}
