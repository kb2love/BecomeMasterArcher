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
        
    }
    public void RecieveDamage(float damage)
    {
        hp -= damage;
        Debug.Log("hp");
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
            playerAttack.IsFind();
        }
    }
}
