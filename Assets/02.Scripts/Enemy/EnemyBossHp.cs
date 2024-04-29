using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBossHp : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private Transform bossTr;
    void Start()
    {
        bossTr = GameObject.Find("BossDragon").transform;
    }
    void Update()
    {
        transform.position = bossTr.position + Vector3.up * 1.25f;
    }
    public void HpDown()
    {
        
    }
}
