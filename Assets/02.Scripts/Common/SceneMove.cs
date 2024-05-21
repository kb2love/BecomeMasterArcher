using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneMove : MonoBehaviour
{
    [SerializeField] private SoundData soundData;                 // ���� ������
    public static SceneMove scenenInst;                           // SceneMove �̱��� �ν��Ͻ�
    private Image loadSceneButIm;                                 // ���� �ε� ��ư �̹���
    private Text loadSceneText;                                   // ���� �ε� ��ư �ؽ�Ʈ
    private int sceneIdx = 1;                                     // ���� �� �ε���
    private TextMeshProUGUI stageNumber;                          // �������� ��ȣ ǥ�� �ؽ�Ʈ
    private Animator animator;                                    // �ִϸ�����
    private readonly string stage = "Stage";                      // �������� �� �̸�
    private readonly string player = "Player";                    // �÷��̾� �� �̸�
    private GameData gameData;                                    // ���� ������

    void Awake()
    {
        if (scenenInst == null)                                   // �̱��� �ν��Ͻ��� ������
        {
            scenenInst = this;                                    // ���� �ν��Ͻ��� �̱��� �ν��Ͻ��� ����
        }
        else if (scenenInst != this)                              // �̱��� �ν��Ͻ��� �̹� �����ϸ�
        {
            Destroy(gameObject);                                  // ���� �ν��Ͻ��� �ı�
            return;                                               // �߰� ������ ���� ���� return
        }

        DontDestroyOnLoad(gameObject);                            // ���� ����Ǿ ������Ʈ �ı����� �ʵ��� ����

        SceneManager.sceneLoaded += OnSceneChanged;               // �� �ε� �Ϸ� �� ȣ��� �޼��� ���
        DataManager.dataInst.LoadData();                          // ������ �ε�
        gameData = DataManager.dataInst.gameData;                 // �ε��� �����Ϳ��� ���� ������ ��������

        loadSceneButIm = GameObject.Find("LoadGameButton (Legacy)").GetComponent<Image>();   // �ε� ��ư �̹��� ������Ʈ ��������
        loadSceneText = loadSceneButIm.transform.GetChild(0).GetComponent<Text>();           // �ε� ��ư �ؽ�Ʈ ������Ʈ ��������

        if (gameData.sceneIdx > 2)                                // �� �ε����� 2���� ũ��
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 255); // ��ư Ȱ��ȭ ���� ����
            loadSceneButIm.GetComponent<Button>().enabled = true; // ��ư Ȱ��ȭ
            loadSceneText.color = new Color32(0, 0, 0, 255);      // �ؽ�Ʈ ���� ����
        }
        else                                                      // �� �ε����� 2 �����̸�
        {
            loadSceneButIm.color = new Color32(0, 255, 255, 50);  // ��ư ��Ȱ��ȭ ���� ����
            loadSceneButIm.GetComponent<Button>().enabled = false;// ��ư ��Ȱ��ȭ
            loadSceneText.color = new Color32(0, 0, 0, 50);       // �ؽ�Ʈ ���� ����
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;               // �� ���� �̺�Ʈ ������ ����
    }

    void Start()
    {
        if (GameObject.Find("StageNumber-Text (TMP)") != null)    // "StageNumber-Text (TMP)" ��ü�� �����ϸ�
        {
            stageNumber = GameObject.Find("StageNumber-Text (TMP)").GetComponent<TextMeshProUGUI>();   // �ؽ�Ʈ ������Ʈ ��������
            animator = stageNumber.gameObject.GetComponent<Animator>();                                // �ִϸ����� ������Ʈ ��������
            stageNumber.text = gameData.stageCount.ToString();                                                  // �������� ��ȣ ����
            animator.SetTrigger("StageStart");                                                         // �ִϸ����� Ʈ���� ����
        }
    }

    public void StartScene()
    {
        SceneManager.LoadScene(0);                               // ù ��° �� �ε�
        DataManager.dataInst.gameData.sceneIdx = sceneIdx = 1;   // �� �ε��� �ʱ�ȭ
        gameData.stageCount = 0;                                          // �������� ī��Ʈ �ʱ�ȭ
        DataManager.dataInst.SaveData();                         // ������ ����
    }

    public void NextStage()
    {
        sceneIdx++;                                              // �� �ε��� ����
        gameData.stageCount++;                                            // �������� ī��Ʈ ����
        gameData.sceneIdx = sceneIdx;                            // ���� �������� �� �ε��� ������Ʈ
        DataManager.dataInst.SaveData();                         // ������ ����

        SceneManager.LoadScene(player);                          // �÷��̾� �� �ε�
        SceneManager.LoadScene(stage, LoadSceneMode.Additive);   // �������� �� �߰��� �ε�

        if (gameData.stageCount == 1 || gameData.stageCount == 5)                  // Ư�� ������������ �߰����� �� �ε�
        {
            SceneManager.LoadScene(player);                      // �÷��̾� �� �ε�
            SceneManager.LoadScene(stage, LoadSceneMode.Additive); // �������� �� �߰��� �ε�
        }
        else if (gameData.stageCount == 10 || sceneIdx == 11)             // ���� �������� ����
        {
            SceneManager.LoadScene(player);                      // �÷��̾� �� �ε�
            SceneManager.LoadScene(11, LoadSceneMode.Additive);  // ���� �������� �� �߰��� �ε�
            SoundManager.soundInst.BackGroundSound(SoundManager.soundInst.GetComponent<AudioSource>(), soundData.bossBGM); // ���� BGM ���
        }
        else                                                     // �Ϲ� ��������
        {
            SceneManager.LoadScene(player);                      // �÷��̾� �� �ε�
            SceneManager.LoadScene(stage, LoadSceneMode.Additive); // �������� �� �߰��� �ε�
            SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive); // ���� �� �ε��� �� �߰��� �ε�
        }

        SoundManager.soundInst.NextStageSound();                 // �������� ��ȯ ���� ���
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Start();                                                 // ���� ����� ������ Start �޼��� ȣ��
    }

    public void SceneLoad()
    {
        DataManager.dataInst.LoadData();                         // ������ �ε�
        sceneIdx = DataManager.dataInst.gameData.sceneIdx;       // �ε��� �����Ϳ��� �� �ε��� ��������
        SceneManager.LoadScene(player);                          // �÷��̾� �� �ε�
        SceneManager.LoadScene(stage, LoadSceneMode.Additive);   // �������� �� �߰��� �ε�
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Additive);// ���� �� �ε��� �� �߰��� �ε�
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;         // �����Ϳ��� ���� ���� ��� �÷��� ��� ����
#else
        Application.Quit();                                      // ����� ���� ����
#endif
        DataManager.dataInst.SaveData();                         // ������ ����
    }
}