using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    [SerializeField] PlayerData playerData;
    private void Awake()
    {
        if (GM == null)
            GM = this;
        else if (GM != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        DataClear();
    }

    private void DataClear()
    {
        playerData.plSpeed = 1.5f;
        playerData.plDamage = 20f;
        playerData.plHP = 100f;
        playerData.plAtcSpeed = 1.0f;
        playerData.plCritical = 0.05f;
        playerData.Exp = 0.0f;
        playerData.MaxExp = 100.0f;
        playerData.isDoubleAtc = false;
    }

    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
