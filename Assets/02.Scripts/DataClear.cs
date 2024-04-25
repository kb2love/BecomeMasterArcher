using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClear : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    void Start()
    {
        playerData.plSpeed = 1.5f;
        playerData.plDamage = 20f;
        playerData.plHP = 100f;
        playerData.plAtcSpeed = 1.0f;
        playerData.plCritical = 0.05f;
        playerData.isDoubleAtc = false;
        GameObject.Find("Player").GetComponent<PlayerAttack>().AttackSpeedUp();
    }

}
