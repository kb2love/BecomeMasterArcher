using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    private float hp;
    void Start()
    {
        hp = _enemyData.Hp;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            other.GetComponent<ArrowCtrl>().ArrowDamage(hp);
            if(hp <=0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
