using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ArrowCtrl : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Rigidbody rb;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50000f * Time.deltaTime);
        Invoke("SetOff", 3.0f);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("123");
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyDamage>().RecieveDamage(playerData.plDamage);
            gameObject.SetActive(false);
        }
    }
    private void SetOff()
    {
        gameObject.SetActive(false);
    }
}
