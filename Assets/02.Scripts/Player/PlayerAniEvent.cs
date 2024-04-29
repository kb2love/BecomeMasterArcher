using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ArrowShot()
    {
        transform.parent.GetComponent<PlayerAttack>().Attack();
    }
}
