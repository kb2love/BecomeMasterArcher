using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="SoundData", menuName = "SoundData", order = 2)]
public class SoundData : ScriptableObject
{
    public AudioClip normalBGM;
    public AudioClip bossBGM;
    public AudioClip arrowClip;
    public AudioClip e_hitClip;
    public AudioClip plHitClip;
    public AudioClip nextStageClip;
    public AudioClip storeOpenClip;
    public AudioClip skillSelect;

}
