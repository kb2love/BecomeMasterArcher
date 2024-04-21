using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] enemies;
    [SerializeField] private List<GameObject> enemiesList;
    private GameObject store;
    private int enemyCount = 0;
    void Start()
    {
        store = GameObject.Find("Store").gameObject;
        enemies = GameObject.Find("Enemies").GetComponentsInChildren<Transform>();
        if(enemies.Length > 1)
        {
            for(int i = 1; i < enemies.Length; i++)
            {
                enemiesList.Add(enemies[i].gameObject);
            }
        }
        enemyCount = enemiesList.Count;
        if(enemyCount == 0)
        {
            store.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f);


        }
    }
    void Update()
    {
        
    }
    public void EnemyDieCount()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            
        }
    }
}
