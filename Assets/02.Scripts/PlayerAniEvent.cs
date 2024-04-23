using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    private Transform attackPos;
    private void Start()
    {
        attackPos = GameObject.Find("AttackPos").transform; 
    }
    public void ArrowShot()
    {
        GameObject arrow = ObjectPoolingManager.objInstance.GetArrow();
        if(arrow != null )
        {
            arrow.transform.position = attackPos.position;
            arrow.transform.rotation = attackPos.rotation;
            arrow.SetActive(true);
        }
    }
}
