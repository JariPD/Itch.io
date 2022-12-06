using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackInfos : MonoBehaviour
{
    public static BlackJackInfos instance;

    [SerializeField] private Animator infoAnim;
    private bool usable = true;

    private void Awake()
    {
        instance = this;
    }

    public void FirstCallClick()
    {
        if (usable)
        {
            infoAnim.Play("Calling");
            usable = false;
        }
    }

    public void CheatCardInfo()
    {
        infoAnim.Play("CheatCards");
    }
}
