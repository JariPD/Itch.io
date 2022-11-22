using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSound : MonoBehaviour
{
    private float timer = 0.5f;

    private void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
            Destroy(this.gameObject);
    }
}
