using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();
    [SerializeField] EnemyData enemyData;
    private Image hpImage;
    private int e_idx;
    void Start()
    {
        hpImage = transform.GetChild(0).GetComponent<Image>();    
        GameObject enemies = GameObject.Find("Enemies").gameObject;
        for(int i = 0; i < enemies.transform.childCount; i++)
        {
            enemiesList.Add(enemies.transform.GetChild(i).gameObject);
        }
        for(int i = 0;i < enemiesList.Count; i++)
        {
            if (enemiesList[i].transform.childCount > 2) continue;
            hpImage.enabled = true;
            GameObject hp = new GameObject("hp");
            hp.transform.SetParent(enemiesList[i].transform);
            e_idx = i;
            if (hpImage.enabled) break;
        }
    }

    void Update()
    {
        if(hpImage.enabled)
        {
            transform.position = enemiesList[e_idx].transform.position + Vector3.up * 0.25f;
            hpImage.fillAmount = enemiesList[e_idx].GetComponent<EnemyDamage>().hp / enemyData.Hp;
            if(enemiesList[e_idx].GetComponent<EnemyDamage>().hp <= 0)
                gameObject.SetActive(false);
        }
    }
}
