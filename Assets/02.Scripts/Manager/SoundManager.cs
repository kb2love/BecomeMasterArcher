using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundInst;
    void Awake()
    {
        if(soundInst == null)
        {
            soundInst = this;
        }
        else if(soundInst != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(soundInst);
    }
    public void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    public void BackGroundSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}
