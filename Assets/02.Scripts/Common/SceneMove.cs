using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneMove : MonoBehaviour
{
    [SerializeField] private SoundData soundData;                 // 사운드 데이터
    public static SceneMove scenenInst;                           // SceneMove 싱글톤 인스턴스
    private Image loadSceneButIm;                                 // 게임 로드 버튼 이미지
    private Text loadSceneText;                                   // 게임 로드 버튼 텍스트
    private int sceneIdx = 1;                                     // 현재 씬 인덱스
    private TextMeshProUGUI stageNumber;                          // 스테이지 번호 표시 텍스트
    private Animator animator;                                    // 애니메이터
    private readonly string stage = "Stage";                      // 스테이지 씬 이름
    private readonly string player = "Player";                    // 플레이어 씬 이름
    private GameData gameData;                                    // 게임 데이터

    void Awake()
    {
        if (scenenInst == null)                                   // 싱글톤 인스턴스가 없으면
        {
            scenenInst = this;                                    // 현재 인스턴스를 싱글톤 인스턴스로 설정
        }
        else if (scenenInst != this)                              // 싱글톤 인스턴스가 이미 존재하면
        {
            Destroy(gameObject);                                  // 현재 인스턴스를 파괴
            return;                                               // 추가 실행을 막기 위해 return
        }

        DontDestroyOnLoad(gameObject);                            // 씬이 변경되어도 오브젝트 파괴되지 않도록 설정

        SceneManager.sceneLoaded += OnSceneChanged;               // 씬 로드 완료 시 호출될 메서드 등록
        DataManager.dataInst.LoadData();                          // 데이터 로드
        gameData = DataManager.dataInst.gameData;                 // 로드한 데이터에서 게임 데이터 가져오기

        loadSceneButIm = GameObject.Find("LoadGameButton (Legacy)").GetComponent<Image>();   // 로드 버튼 이미지 컴포넌트 가져오기
        loadSceneText = loadSceneButIm.transform.GetChild(0).GetComponent<Text>();           // 로드 버튼 텍스트 컴포넌트 가져오기

        if (gameData.sceneIdx > 2)                                // 씬 인덱스가 2보다 크면
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 255); // 버튼 활성화 색상 설정
            loadSceneButIm.GetComponent<Button>().enabled = true; // 버튼 활성화
            loadSceneText.color = new Color32(0, 0, 0, 255);      // 텍스트 색상 설정
        }
        else                                                      // 씬 인덱스가 2 이하이면
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 50);  // 버튼 비활성화 색상 설정
            loadSceneButIm.GetComponent<Button>().enabled = false;// 버튼 비활성화
            loadSceneText.color = new Color32(0, 0, 0, 50);       // 텍스트 색상 설정
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;               // 씬 변경 이벤트 리스너 해제
    }

    void Start()
    {
        if (GameObject.Find("StageNumber-Text (TMP)") != null)    // "StageNumber-Text (TMP)" 객체가 존재하면
        {
            stageNumber = GameObject.Find("StageNumber-Text (TMP)").GetComponent<TextMeshProUGUI>();   // 텍스트 컴포넌트 가져오기
            animator = stageNumber.gameObject.GetComponent<Animator>();                                // 애니메이터 컴포넌트 가져오기
            stageNumber.text = gameData.stageCount.ToString();                                                  // 스테이지 번호 설정
            animator.SetTrigger("StageStart");                                                         // 애니메이터 트리거 설정
        }
    }

    public void StartScene()
    {
        SceneManager.LoadScene(0);                               // 첫 번째 씬 로드
        DataManager.dataInst.gameData.sceneIdx = sceneIdx = 1;   // 씬 인덱스 초기화
        gameData.stageCount = 0;                                          // 스테이지 카운트 초기화
        DataManager.dataInst.SaveData();                         // 데이터 저장
    }

    public void NextStage()
    {
        sceneIdx++;                                              // 씬 인덱스 증가
        gameData.stageCount++;                                            // 스테이지 카운트 증가
        gameData.sceneIdx = sceneIdx;                            // 게임 데이터의 씬 인덱스 업데이트
        DataManager.dataInst.SaveData();                         // 데이터 저장

        SceneManager.LoadScene(player);                          // 플레이어 씬 로드
        SceneManager.LoadScene(stage, LoadSceneMode.Additive);   // 스테이지 씬 추가로 로드

        if (gameData.stageCount == 1 || gameData.stageCount == 5)                  // 특정 스테이지에서 추가적인 씬 로드
        {
            SceneManager.LoadScene(player);                      // 플레이어 씬 로드
            SceneManager.LoadScene(stage, LoadSceneMode.Additive); // 스테이지 씬 추가로 로드
        }
        else if (gameData.stageCount == 10 || sceneIdx == 11)             // 보스 스테이지 조건
        {
            SceneManager.LoadScene(player);                      // 플레이어 씬 로드
            SceneManager.LoadScene(11, LoadSceneMode.Additive);  // 보스 스테이지 씬 추가로 로드
            SoundManager.soundInst.BackGroundSound(SoundManager.soundInst.GetComponent<AudioSource>(), soundData.bossBGM); // 보스 BGM 재생
        }
        else                                                     // 일반 스테이지
        {
            SceneManager.LoadScene(player);                      // 플레이어 씬 로드
            SceneManager.LoadScene(stage, LoadSceneMode.Additive); // 스테이지 씬 추가로 로드
            SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive); // 현재 씬 인덱스 씬 추가로 로드
        }

        SoundManager.soundInst.NextStageSound();                 // 스테이지 전환 사운드 재생
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Start();                                                 // 씬이 변경될 때마다 Start 메서드 호출
    }

    public void SceneLoad()
    {
        DataManager.dataInst.LoadData();                         // 데이터 로드
        sceneIdx = DataManager.dataInst.gameData.sceneIdx;       // 로드한 데이터에서 씬 인덱스 가져오기
        SceneManager.LoadScene(player);                          // 플레이어 씬 로드
        SceneManager.LoadScene(stage, LoadSceneMode.Additive);   // 스테이지 씬 추가로 로드
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive);// 현재 씬 인덱스 씬 추가로 로드
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;         // 에디터에서 실행 중일 경우 플레이 모드 종료
#else
        Application.Quit();                                      // 빌드된 게임 종료
#endif
        DataManager.dataInst.SaveData();                         // 데이터 저장
    }
}