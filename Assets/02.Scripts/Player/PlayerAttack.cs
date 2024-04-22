using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Animator animator;
    private TouchPad touchPad;
    private Transform[] enemies;
    private List<Transform> enemiesList = new List<Transform>();
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        enemies = GameObject.Find("Enemies").GetComponentsInChildren<Transform>();
        for(int i = 1; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i]);
        }
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {//¿òÁ÷ÀÏ‹š °ø°Ýx
            
        }
        else
        {//¸ØÃç¼­ °ø°Ýo
            animator.SetBool("IsWalk", false);
            animator.SetTrigger("AttackTrigger");
        }
        
    }
}
