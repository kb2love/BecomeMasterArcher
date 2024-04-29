using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SoundData soundData;
    private AudioSource source;
    private Image expImage;
    private Image hpImage;
    private float h = 0, v = 0;
    private float mvSpeed;
    private CharacterController ch;
    private Animator animator;
    private GameData gameData;
    private Vector3 frontDir;
    public void OnStickPos(Vector3 stickPos, Vector3 diffVec)
    {
        h = stickPos.x;
        v = stickPos.z;
        frontDir = new Vector3(diffVec.x, 0, diffVec.y);
    }
    void Start()
    {
        animator =  GetComponentInChildren<Animator>();
        ch = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        gameData = DataManager.dataInst.gameData;
        expImage = GameObject.Find("ExpImage").GetComponent<Image>();
        mvSpeed = DataManager.dataInst.gameData.plSpeed;
        expImage.fillAmount = gameData.Exp / gameData.MaxExp;
        hpImage = GameObject.Find("PlayerHp_Image").transform.GetChild(0).GetComponent<Image>();
        hpImage.fillAmount = gameData.plHP / gameData.plMaxHP;
    }

    void Update()
    {
        float qwe = 0;
            qwe = Mathf.Abs(h) + Mathf.Abs(v);
        animator.SetFloat("moveSpeed", qwe);
        if(frontDir.magnitude > 0.1f)
        {
            Quaternion dir = Quaternion.LookRotation((frontDir).normalized);
            dir.z = dir.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 10f * Time.deltaTime);
        }
        Vector3 moveDir = new Vector3(h * Time.deltaTime * mvSpeed, 0, v * Time.deltaTime * mvSpeed);
        ch.Move(moveDir);

    }
    public void Heal(float heal)
    {
        gameData.plHP += heal;
        if(gameData.plHP >= gameData.plMaxHP)
            gameData.plHP = gameData.plMaxHP;
        hpImage.fillAmount = gameData.plHP / gameData.plMaxHP;
    }
    public void PlayerRecieveDamage(float damage)
    {
        gameData.plHP -= damage;
        hpImage.fillAmount = gameData.plHP / gameData.plMaxHP;
        source.PlayOneShot(soundData.plHitClip);
        HitEff();
        Camera.main.GetComponent<CameraMove>().Shake();
        if (gameData.plHP <= 0)
        {
            StartCoroutine(playerDie());
        }
    }
    IEnumerator playerDie()
    {
        source.PlayOneShot(soundData.dieClip);
        Time.timeScale = 0;
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1;
        SceneMove.scenenInst.StartScene();
    }
    private void HitEff()
    {
        GameObject hitEff = ObjectPoolingManager.objInstance.GetHitEff();
        hitEff.transform.position = transform.position;
        hitEff.transform.rotation = transform.rotation;
        hitEff.SetActive(true);
        DOTween.Sequence()
        .AppendCallback(() => hitEff.SetActive(true))
        .AppendInterval(0.15f)
        .AppendCallback(() => hitEff.SetActive(false))
        .SetUpdate(true);
    }
    public void PlayerExpUp(int exp)
    {
        gameData.Exp += exp * 30f;
        expImage.fillAmount = gameData.Exp / gameData.MaxExp;
        if (gameData.Exp >= gameData.MaxExp)
        {
            GameObject.Find("SkillStore").transform.GetChild(0).GetComponent<SkillManager>().SkillStoreOpen();
            gameData.Exp -= gameData.MaxExp;
            expImage.fillAmount = gameData.Exp / gameData.MaxExp;
        }

    }
}
