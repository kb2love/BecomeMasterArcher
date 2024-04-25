using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public static SceneMove scenenInst;
    private int sceneIdx = 0;
    private TextMeshProUGUI stageNumber;
    private Animator animator;
    
    void Awake()
    {
        if (scenenInst == null)
            scenenInst = this;
        else if (scenenInst != this)
            Destroy(scenenInst);
        DontDestroyOnLoad(scenenInst);
        SceneManager.sceneLoaded += OnSceneChanged;
    }
    void OnDisable()
    {
        // 씬 변경 이벤트 리스너 해제
        SceneManager.sceneLoaded -= OnSceneChanged;
    }
    public void Start()
    {
        stageNumber = GameObject.Find("StageNumber-Text (TMP)").GetComponent<TextMeshProUGUI>();
        animator = stageNumber.gameObject.GetComponent<Animator>();
        stageNumber.text = (sceneIdx + 1).ToString();
        animator.SetTrigger("StageStart");
        Debug.Log(123);
    }

    public void NextScene()
    {
        sceneIdx++;
        SceneManager.LoadScene(sceneIdx);
        SoundManager.soundInst.NextStageSound();
    }
    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    { 
        Start();
    }
    public void SceneLoad()
    {
        SceneManager.LoadScene(sceneIdx);
    }
}
