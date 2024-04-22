using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="SkillData", menuName ="SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public Sprite attackSpeed_Im;
    public Sprite attackPower_Im;
    public Sprite hpUp_Im;
    public Sprite criticalUp_Im;
    public Sprite doubleAttack_Im;
}
