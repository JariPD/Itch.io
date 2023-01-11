using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public void PlayAudio(string AudioName)
    {
        AudioManager.instance.Play(AudioName);
    }

    public void PlayAudioInGameObject()
    {
        GetComponent<AudioSource>().Play();
    }
}
