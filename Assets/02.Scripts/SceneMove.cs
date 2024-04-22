using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public static SceneMove scenenInst;
    private int sceneIdx = 0;
    void Awake()
    {
        if (scenenInst == null)
            scenenInst = this;
        else if (scenenInst != this)
            Destroy(scenenInst);
        DontDestroyOnLoad(scenenInst);
    }
    public void NextScene()
    {
        sceneIdx++;
        SceneManager.LoadScene(sceneIdx);
    }
    public void SceneLoad()
    {
        SceneManager.LoadScene(sceneIdx);
    }
}
