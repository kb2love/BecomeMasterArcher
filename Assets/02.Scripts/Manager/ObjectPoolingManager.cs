using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objInstance;
    [SerializeField] private PlayerData playerData;
    [SerializeField] protected EnemyData enemyData;
    protected List<GameObject> hitEffList = new List<GameObject>();
    private List<GameObject> arrowList = new List<GameObject>();
    private void Awake()
    {
        objInstance = this;
    }
    void Start()
    {
        CreatArrow();
        CreatHitEff();
    }

    private void CreatArrow()
    {
        GameObject arrowGroup = new GameObject("ArrowGroup");
        for (int i = 0; i < 20; i++)
        {
            GameObject arrow = Instantiate(playerData.arrow, arrowGroup.transform);
            arrow.name = (i + 1).ToString() + "°³";
            arrow.gameObject.SetActive(false);
            arrowList.Add(arrow);
        }
    }
    private void CreatHitEff()
    {
        GameObject hitEffGroup = new GameObject("HitEffGroup");
        for (int i = 0; i < 10; i++)
        {
            GameObject hitEff= Instantiate(enemyData.hitEff, hitEffGroup.transform);
            hitEff.name = (i + 1).ToString() + "°³";
            hitEff.gameObject.SetActive(false);
            hitEffList.Add(hitEff);
        }
    }
    public GameObject GetArrow()
    {
        foreach(GameObject arrows in arrowList)
        {
            if(!arrows.activeSelf)
                return arrows;
        }
        return null;
    }
    public GameObject GetHitEff()
    {
        foreach(GameObject hitEff in hitEffList)
        {
            if(!hitEff.activeSelf) return hitEff;
        }
        return null;
    }
}
