using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5.0f;
    public float h = 0, v = 0;
    private Animator animator;
    private Vector3 frontDir;
    public void OnStickPos(Vector3 stickPos, Vector3 diffVec)
    {
        h = stickPos.x;
        v = stickPos.z;
        frontDir = new Vector3(diffVec.x, 0, diffVec.y);
    }
    void Start()
    {
        animator =  GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float qwe = 0;
            qwe = Mathf.Abs(h) + Mathf.Abs(v);
        animator.SetFloat("moveSpeed", qwe);
        if(frontDir.magnitude > 0.1f)
        {
            Quaternion dir = Quaternion.LookRotation((frontDir).normalized);
            dir.z = dir.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 10f * Time.deltaTime);
        }
        transform.Translate(h * Time.deltaTime, 0, v * Time.deltaTime, Space.World);

    }
}
