using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="EnemyData", menuName ="EnemyData", order = 2)]
public class EnemyData : ScriptableObject
{
    public float Hp;
    public float Damage;
    public GameObject hitEff;
}
