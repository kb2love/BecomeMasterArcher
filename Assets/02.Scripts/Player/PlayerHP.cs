using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private Transform plTr;

    void Start()
    {
        plTr = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if(gameObject.activeSelf)
        {
            transform.position = plTr.position + Vector3.up * 1.25f;
        }
    }
}
