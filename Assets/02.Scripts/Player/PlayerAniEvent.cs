using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    private void Start()
    {

    }
    public void ArrowShot()
    {
        transform.parent.GetComponent<PlayerAttack>().Attack();
    }
}
