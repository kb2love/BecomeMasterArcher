using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;

public class ArrowCtrl : MonoBehaviour
{
    private GameData gameData;
    private Tweener tweener;
    void OnEnable()
    {
        if(tweener != null)
            tweener.Kill();
        tweener = transform.DOLocalMove(transform.forward * 10f, 2.5f).OnComplete(SetOff);
        gameData = DataManager.dataInst.gameData;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            float randomValue = Random.value;
            if(randomValue < gameData.plCritical)
            {
                other.GetComponent<EnemyDamage>().RecieveDamage(gameData.plDamage * 2);
            }
            else
            {
                other.GetComponent<EnemyDamage>().RecieveDamage(gameData.plDamage);
            }
            gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "BossDragon")
        {
            float randomValue = Random.value;
            if (randomValue < gameData.plCritical)
            {
                other.GetComponent<EnemyBossDamage>().RecieveDamage(gameData.plDamage * 2);
            }
            else
            {
                other.GetComponent<EnemyBossDamage>().RecieveDamage(gameData.plDamage);
            }
            gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        transform.position = Vector3.zero;
    }
    private void SetOff()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
}
