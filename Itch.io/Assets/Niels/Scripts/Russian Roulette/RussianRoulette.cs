using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RussianRoulette : MonoBehaviour
{
    public static RussianRoulette instance;

    public Transform[] BulletPoints;

    public Rigidbody rb;

    [SerializeField] private Button shoot; 
    [SerializeField] private Button spin; 

    private GunBarrel gunBarrel;

    [SerializeField] private Animator gunPointing;
    [SerializeField] private Animator gunAnim;


    private bool hasShot = false;
    private bool hasSpin = false;
    public bool opponentTurn = false;

    private void Awake()
    {
        instance = this;
        gunBarrel = FindObjectOfType<GunBarrel>();
    }

    void Start()
    {
        gunAnim = GetComponent<Animator>();
        ChooseBulletHolder();
    }

    void Update()
    {
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, new Vector3(0, 0, 0), 1 * Time.deltaTime);
        CheckShot();
        SpinShoot();
    }

    public void SpinAnim()
    {
        if (RouletteAI.instance.MyTurn == false)
            Usable();

        gunAnim.Play("GunSpin");
    }

    public void Spin()
    {
        rb.AddTorque(Vector3.back * Random.Range(1500, 2500));
        gunBarrel.Playit();

        hasSpin = true;
    }

    public void ShootAnim()
    {
        StartCoroutine(PointAndShoot("PlayerPoint"));
        if (RouletteAI.instance.MyTurn == false)
            Usable();
    }

    public void Shoot()
    {
        if (gunBarrel.point.GetComponent<BulletPoint>().HasBullet)
            for (int i = 0; i < gunBarrel.Shoot.Length; i++)
                gunBarrel.Shoot[i].Play();
        else
            Debug.Log("Click");
    }

    private void ChooseBulletHolder()
    {
        BulletPoints[Random.Range(0, BulletPoints.Length)].gameObject.GetComponent<BulletPoint>().HasBullet = true;
    }

    /// <summary>
    /// check if spinning barrel is done then after that it shoots the person
    /// </summary>
    public void SpinShoot()
    {
        if (hasSpin)
        {
            if (gunAnim.GetCurrentAnimatorClipInfo(0).Length <= 0)
            {
                if (rb.angularVelocity.x == 0)
                {
                    if (!gunPointing.IsInTransition(0))
                    {
                        if (opponentTurn)
                            RouletteAI.instance.ON();

                        if (RouletteAI.instance.MyTurn)
                            StartCoroutine(PointAndShoot("OpponentPoint"));
                        else if (RouletteAI.instance.MyTurn == false)
                            StartCoroutine(PointAndShoot("PlayerPoint"));

                        gunPointing.CrossFade("Default", 1);

                        hasSpin = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if shooting animation is done so it switches
    /// </summary>
    private void CheckShot()
    {
        if (hasShot)
        {
            if (gunAnim.GetCurrentAnimatorClipInfo(0).Length <= 0)
            {
                opponentTurn = !opponentTurn;

                gunPointing.CrossFade("Default", 1);
                if (gunPointing.GetCurrentAnimatorClipInfo(0).Length <= 0)
                {
                    if (opponentTurn)
                        RouletteAI.instance.AITurnOn();
                    else
                        Usable();


                    hasShot = false;
                }
            }
        }
    }

    public void Usable()
    {
        shoot.interactable = !shoot.interactable;
        spin.interactable = !spin.interactable;
    }


    /// <summary>
    /// shoot person with string name
    /// </summary>
    /// <param name="PointTo"></param>
    /// <returns></returns>
    public IEnumerator PointAndShoot(string PointTo)
    {
        gunPointing.Play(PointTo);
        yield return new WaitForSeconds(Random.Range(1, 5));
        gunAnim.Play("GunShoot");
        hasShot = true;
    }
}
