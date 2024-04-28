using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public static SceneMove scenenInst;
    private int sceneIdx = 1;
    private int sceneTextIdx = 0;
    private TextMeshProUGUI stageNumber;
    private Animator animator;
    private string stage = "Stage";
    private string player = "Player";
    
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
        if(GameObject.Find("StageNumber-Text (TMP)") != null)
        {
            stageNumber = GameObject.Find("StageNumber-Text (TMP)").GetComponent<TextMeshProUGUI>();
            animator = stageNumber.gameObject.GetComponent<Animator>();
            stageNumber.text = (sceneTextIdx).ToString();
            animator.SetTrigger("StageStart");
        }
    }

    public void NextStage()
    {
        sceneIdx++;
        sceneTextIdx++;
        if(sceneTextIdx == 1)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);
        }
        else if(sceneTextIdx == 5)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);  
        }
        else
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive);
        }
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
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
