using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrel : MonoBehaviour
{
    [SerializeField] private RussianRoulette russianRouletteBarrel;
    [SerializeField] private Transform parentRot;

    [SerializeField] private AudioSource barrelSound;

    private bool on = false;

    public Transform point;

    private float degree;

    public ParticleSystem[] Shoot;

    private void Awake()
    {
        russianRouletteBarrel = FindObjectOfType<RussianRoulette>();
        barrelSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (russianRouletteBarrel.rb.angularVelocity.z < -0.4f && russianRouletteBarrel.rb.angularVelocity.z > -0.5f)
        {
            point = GetClosestEnemy(russianRouletteBarrel.BulletPoints);
            StartCoroutine(Test(point));
            on = false;
        }

        if (on == false)
            parentRot.localRotation = Quaternion.RotateTowards(parentRot.localRotation, Quaternion.Euler(0, 0, degree), 1 * Time.maximumDeltaTime);
    }

    public void Playit()
    {
        on = true;
    }

    Transform GetClosestEnemy(Transform[] _bulletPoints)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in _bulletPoints)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletPoint"))
            if (!barrelSound.isPlaying)
                if (other.GetComponent<BulletPoint>().HasBullet)
                {
                    barrelSound.pitch = 3.5f;
                    barrelSound.Play(0);
                }
                else
                {
                    barrelSound.pitch = 3;
                    barrelSound.Play(0);
                }
    }

    public IEnumerator Test(Transform testtrans)
    {
        degree = testtrans.GetComponent<BulletPoint>().degrees;
        StartCoroutine(CheckForDegree());
        Debug.Log("dic");
        yield return new WaitForSeconds(1);
    }

    public IEnumerator CheckForDegree()
    {
        on = false;

        yield return new WaitForSeconds(1);
    }
}
