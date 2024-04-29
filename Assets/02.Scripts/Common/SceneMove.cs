using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneMove : MonoBehaviour
{
    [SerializeField] SoundData soundData;
    public static SceneMove scenenInst;
    private Image loadSceneButIm;
    private Text loadSceneText;
    private int sceneIdx = 1;
    private int stageCount = 0;
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
        SceneManager.sceneLoaded += OnSceneChanged;
        DataManager.dataInst.LoadData();
        gameData = DataManager.dataInst.gameData;
        loadSceneButIm = GameObject.Find("LoadGameButton (Legacy)").GetComponent<Image>();
        loadSceneText = loadSceneButIm.transform.GetChild(0).GetComponent<Text>();
        if (gameData.sceneIdx > 2)
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 255);
            loadSceneButIm.GetComponent<Button>().enabled = true;
            loadSceneText.color = new Color32(0, 0, 0, 255);
        }
        else
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 50);
            loadSceneButIm.GetComponent<Button>().enabled = false;  
            loadSceneText.color = new Color32(0, 0, 0, 50);
        }
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
            stageNumber.text = (stageCount).ToString();
            animator.SetTrigger("StageStart");
        }
    }
    public void StartScene()
    {
        SceneManager.LoadScene(0);
        DataManager.dataInst.gameData.sceneIdx = sceneIdx = 1;
        stageCount = 0;
        DataManager.dataInst.SaveData();
    }
    public void NextStage()
    {
        sceneIdx++;
        stageCount++;
        gameData.sceneIdx = sceneIdx;
        DataManager.dataInst.SaveData();
        if(stageCount == 1)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);
        }
        else if(stageCount == 5)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(stage, LoadSceneMode.Additive);
        }
        else if (stageCount == 10 || sceneIdx == 11)
        {
            SceneManager.LoadScene(player);
            SceneManager.LoadScene(11, LoadSceneMode.Additive);
            SoundManager.soundInst.BackGroundSound(SoundManager.soundInst.GetComponent<AudioSource>(), soundData.bossBGM);
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
        sceneIdx = DataManager.dataInst.gameData.sceneIdx;
        stageCount = sceneIdx + 1;
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
