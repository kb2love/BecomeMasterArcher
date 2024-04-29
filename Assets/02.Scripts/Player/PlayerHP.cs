using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private GameData gameData;
    private Transform plTr;
    private Image hpImage;

    void Start()
    {
        plTr = GameObject.Find("Player").transform;
        gameData = DataManager.dataInst.gameData;
        hpImage = transform.GetChild(0).GetComponent<Image>();
        hpImage.fillAmount = gameData.plHP / gameData.plMaxHP;
    }
    void Update()
    {
        if(gameObject.activeSelf)
        {
            transform.position = plTr.position + Vector3.up * 1.25f;
        }
    }
    public void HpDown()
    {
        hpImage.fillAmount = gameData.plHP / gameData.plMaxHP;
        if (gameData.plHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
