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
        Vector3 asd = new Vector3(0, 0, 1);
        asd = transform.TransformDirection(asd);
        rb.AddForce(asd * 50000f * Time.deltaTime);
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
}
