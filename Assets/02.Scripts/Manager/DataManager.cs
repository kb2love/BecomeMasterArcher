using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.iOS;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInst;
    public GameData gameData;
    private string path;
    private string fileName = "GameData";
    void Awake()
    {
        if (dataInst == null)
            dataInst = this;
        else if (dataInst != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        gameData = new GameData();
        path = Path.Combine(Application.persistentDataPath, fileName);
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, data);
        Debug.Log(data);  
        Debug.Log(fileName + path);
    }
    public void LoadData()
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(data);
        }
    }
}
public class GameData
{
    public float plSpeed;
    public float plDamage;
    public float plHP;
    public float plMaxHP;
    public float plAtcSpeed;
    public float plCritical;
    public float Exp;
    public float MaxExp;
    public bool isDoubleAtc;
    public int sceneIdx;
}
