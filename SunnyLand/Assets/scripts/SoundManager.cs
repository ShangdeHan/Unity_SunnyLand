using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio, hurtAudio, collectionAudio, gameoverAudio;
    public AudioSource BGM;

    private void Awake()
    {
        instance = this;
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void ColleAudio()
    {
        audioSource.clip = collectionAudio;
        audioSource.Play();
    }

    public void OverAudio()
    {
        audioSource.clip = gameoverAudio;
        audioSource.Play();
    }
}
