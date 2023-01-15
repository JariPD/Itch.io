using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteAI : MonoBehaviour
{
    public static RouletteAI instance;

    [SerializeField] public bool MyTurn { get; private set; }

    private void Awake()
    {
        instance = this;
        MyTurn = false;
    }

    public void ON()
    {
        MyTurn = true;
    }

    public void AITurnOn()
    {
        MyTurn = true;
        StartCoroutine(AIPlay());
    }

    public void AISwitch()
    {
        MyTurn = !MyTurn;
    }

    private IEnumerator AIPlay()
    {
        int chance = Random.Range(0, 2);
        yield return new WaitForSeconds(Random.Range(2, 4));
        if (chance == 0)
            RussianRoulette.instance.Spinp();
        else if (chance == 1)
            StartCoroutine(RussianRoulette.instance.PointAndShoot("OpponentPoint"));
    }
}
