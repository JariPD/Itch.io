using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public Transform Position;

    [Range(0f, 1f)]
    public float Volume = 1;

    [Range(-3f, 3f)]
    public float Pitch = 1;

    [Range(0, 1)]
    public float Spatial = 1;

    [Range(1, 1000)]
    public float MaxDistance = 500;

    public bool Loop = false;
    public bool PlayOnAwake = true;

    [HideInInspector]
    public AudioSource source;
}
