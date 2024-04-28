using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "playerData", menuName = "playerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public float plSpeed;
    public float plDamage;
    public float plHP;
    public float plAtcSpeed;
    public float plCritical;
    public float Exp;
    public float MaxExp;
    public bool isDoubleAtc;
    public GameObject arrow;
}
