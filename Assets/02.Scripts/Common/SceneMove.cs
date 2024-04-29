using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private GameData gameData;
    void Awake()
    {
        if (scenenInst == null)
            scenenInst = this;
        else if (scenenInst != this)
            Destroy(scenenInst);
        DontDestroyOnLoad(scenenInst);
        gameData = DataManager.dataInst.gameData;
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
    public void StartScene()
    {
        SceneManager.LoadScene(0);
        sceneIdx = 0;
        sceneTextIdx = 0;
    }
    public void NextStage()
    {
        sceneIdx++;
        sceneTextIdx++;
        gameData.sceneIdx = sceneIdx;
        DataManager.dataInst.SaveData();
        if(sceneTextIdx == 1)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);
        }
        else if(sceneTextIdx == 3)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(10, LoadSceneMode.Additive);
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
        DataManager.dataInst.LoadData();
        SceneManager.LoadScene(player);
        SceneManager.LoadScene(stage, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        DataManager.dataInst.SaveData();
    }
}
