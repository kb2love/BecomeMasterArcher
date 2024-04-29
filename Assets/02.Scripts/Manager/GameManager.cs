using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    private GameData gameData;
    private void Awake()
    {
        if (GM == null)
            GM = this;
        else if (GM != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        gameData = DataManager.dataInst.gameData;
        DataClear();
    }
    private void DataClear()
    {
        gameData.plSpeed = 1.5f;
        gameData.plDamage = 20f;
        gameData.plHP = 100f;
        gameData.plMaxHP = 100f;
        gameData.plAtcSpeed = 1.0f;
        gameData.plCritical = 0.05f;
        gameData.Exp = 0.0f;
        gameData.MaxExp = 100.0f;
        gameData.isDoubleAtc = false;
    }

}
