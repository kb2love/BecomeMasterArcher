using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundInst;
    private AudioSource source;
    [SerializeField] SoundData soundData;
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
        source = GetComponent<AudioSource>();

    }
    private void Start()
    {
        BackGroundSound(source, soundData.normalBGM);
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
    public void NextStageSound()
    {
        source.PlayOneShot(soundData.nextStageClip, 4.0f);
    }
}
