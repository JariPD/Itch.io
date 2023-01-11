using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] Sounds;
    public Reverb Reverbs;

    private void Awake()
    {
        instance = this;

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //s.source.transform.position = s.Position.transform.position;

            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.spatialBlend = s.Spatial;
            s.source.maxDistance = s.MaxDistance;

            s.source.loop = s.Loop;
            s.source.playOnAwake = s.PlayOnAwake;
        }

        Reverbs.source = gameObject.AddComponent<AudioReverbFilter>();
        Reverbs.source.reverbPreset = Reverbs.Preset;
    }
    private void Start()
    {
        //Play("BackGround");
    }

    private void Update()
    {
        for (int i = 0; i < Sounds.Length; i++)
            if (Sounds[i].source.transform.position != null)
                Sounds[i].source.transform.position = Sounds[i].Position.transform.position;
    }

    public void Play(string _name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == _name);
        if (s == null)
        {
            print(name);
            print("Sound not Found");
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
        s.source.volume = Mathf.Lerp(s.source.volume, s.Volume, 1 * Time.deltaTime);
    }

    public void StopPlaying(string _name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == _name);
        if (s == null)
            return;

        if (s.source.loop == true)
            s.source.loop = false;

        s.source.volume = Mathf.MoveTowards(s.source.volume, 0, 0.5f * Time.deltaTime);
    }

    public void ReverbPreset(AudioReverbPreset _preset)
    {
        Reverbs.source.reverbPreset = _preset;
    }

}
