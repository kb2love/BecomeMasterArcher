using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTr;

    void Start()
    {
        playerTr = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Vector3 plTr = new Vector3(0, 5.1f, playerTr.position.z - 1.5f);
        transform.position = plTr;
    }
}
