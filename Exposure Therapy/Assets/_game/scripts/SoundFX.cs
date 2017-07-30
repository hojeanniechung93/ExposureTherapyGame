using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private static SoundFX _instance;
    private static SoundFX Instance { get { return _instance; } }

    [Header("Environment")]
    public AudioClip wind;

    [Header("Walking")]
    public AudioClip grassWalk;
    public AudioClip woodWalk;
    public AudioClip portalWalk;

    [Header("Targets")]
    public AudioClip collectTarget;

    [Header("Warning")]
    public AudioClip warningWind;

    [Header("Jumping")]
    public AudioClip jumpStart;
    public AudioClip jumpEnd;

	[Header("Ending")]
	public AudioClip EndingClip;

	public static AudioClip EndScene{get { return Instance.EndingClip;}}

    public static AudioClip Wind { get { return Instance.wind; } }
    public static AudioClip GrassWalk { get { return Instance.grassWalk; } }
    public static AudioClip WoodWalk { get { return Instance.woodWalk; } }
    public static AudioClip CollectTarget { get { return Instance.collectTarget; } }
    public static AudioClip WarningWind { get { return Instance.warningWind; } }
    public static AudioClip PortalWalk { get { return Instance.portalWalk; } }
    public static AudioClip JumpStart { get { return Instance.jumpStart; } }
    public static AudioClip JumpEnd { get { return Instance.jumpEnd; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void PlayAudioSource(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.Play();
    }

    public static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public static void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}



