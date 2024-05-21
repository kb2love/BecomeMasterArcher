using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalOpen : MonoBehaviour
{
    private string playerName = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerName))
        {
            SceneMove.scenenInst.NextStage();
        }
    }
}
