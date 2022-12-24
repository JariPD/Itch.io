using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Animator play;
    public GameObject Glow;
    public bool LeftRight = false;

    public bool off = false;

    private void Awake()
    {
        play = GetComponent<Animator>();
    }

    private void Update()
    {
        if (off)
        {
            Glow.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (off == false)
        {
            play.SetBool("On", true);
            Glow.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        if (off == false)
        {
            MainMenuManager.instance.PlayMenu();
            off = true;
        }

        if (LeftRight == false)
        {
            Application.Quit();
        }
    }

    private void OnMouseExit()
    {
        play.SetBool("On", false);
        Glow.SetActive(false);
    }
}
