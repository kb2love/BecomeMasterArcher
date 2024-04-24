using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objInstance;
    [SerializeField] private PlayerData playerData;
    private List<GameObject> arrowList = new List<GameObject>();
    private void Awake()
    {
        objInstance = this;
    }
    void Start()
    {
        GameObject arrowGroup = new GameObject("ArrowGroup");
        for(int i = 0; i < 20; i++)
        {
            GameObject arrow = Instantiate(playerData.arrow,arrowGroup.transform);
            arrow.name = (i+1).ToString() + "°³";
            arrow.gameObject.SetActive(false);
            arrowList.Add(arrow);
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
    void Update()
    {
        
    }
}
