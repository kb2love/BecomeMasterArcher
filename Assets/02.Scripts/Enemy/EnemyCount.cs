using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    private int enemyDieIdx = 0;
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
        enemyDieIdx++;
        Debug.Log(enemyDieIdx);
        if (enemies.Count == 0)
        {
            SkillManager.skillInst.PlayerExpUp(enemyDieIdx);
            GameObject.Find("Door").transform.GetChild(0).gameObject.SetActive(true);
            enemyDieIdx = 0;
        }
    }
}
