using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reverb
{
    public AudioReverbPreset Preset;

    [HideInInspector]
    public AudioReverbFilter source;
}