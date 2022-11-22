using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RussianRoulette : MonoBehaviour
{
    public Transform[] BulletPoints;

    public Rigidbody rb;

    [SerializeField] private Button shoot; 
    [SerializeField] private Button spin; 

    private GunBarrel gunBarrel;

    private Animator gunAnim;

    private void Awake()
    {
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
    }

    public void Usable()
    {
        shoot.interactable = !shoot.interactable;
        spin.interactable = !spin.interactable;
    }

    public void SpinAnim()
    {
        gunAnim.Play("GunSpin");
    }

    public void Spin()
    {
        rb.AddTorque(Vector3.back * Random.Range(1500, 2500));
        gunBarrel.Playit();
        StartCoroutine(UsableSwitch());
    }

    public void ShootAnim()
    {
        gunAnim.Play("GunShoot");
    }

    public void Shoot()
    {
        if (gunBarrel.point.GetComponent<BulletPoint>().HasBullet)
            for (int i = 0; i < gunBarrel.Shoot.Length; i++)
                gunBarrel.Shoot[i].Play();
        else
            Debug.Log("Click");

        StartCoroutine(UsableSwitch());
    }

    private void ChooseBulletHolder()
    {
        BulletPoints[Random.Range(0, BulletPoints.Length)].gameObject.GetComponent<BulletPoint>().HasBullet = true;
    }

    private IEnumerator UsableSwitch()
    {
        Usable();
        yield return new WaitForSeconds(4);
        Usable();
    }
}
