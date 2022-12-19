using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraSettings : MonoBehaviour
{
    public static VirtualCameraSettings instance;

    private CinemachineVirtualCamera vcam;

    private void Awake()
    {
        instance = this;
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Hit(float time)
    {
        StartCoroutine(HitMove(time));
    }

    private IEnumerator HitMove(float time)
    {
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 50;
        yield return new WaitForSeconds(time);
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;

    }
}
