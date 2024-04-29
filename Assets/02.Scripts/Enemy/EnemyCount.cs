using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    void Start()
    {
        GameObject enemy = GameObject.Find("Enemies").gameObject;
        for(int i = 0; i < enemy.transform.childCount; i++)
        {
            enemies.Add(enemy.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }
    public void EnemyCountDown(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            GameObject.Find("Door").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
