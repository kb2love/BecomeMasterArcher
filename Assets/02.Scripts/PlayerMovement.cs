using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5.0f;
    public float h = 0, v = 0;
    private Rigidbody rb;
    private Animator animator;
    public void OnStickPos(Vector3 stickPos)
    {
        h = stickPos.x;
        v = stickPos.z;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(h));
        transform.Translate(h * Time.deltaTime, 0, v * Time.deltaTime, Space.World);
    }
}
