using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] SoundData soundData;
    private Image expImage;
    private float h = 0, v = 0;
    private float mvSpeed;
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
        gameData = DataManager.dataInst.gameData;
        expImage = GameObject.Find("ExpImage").GetComponent<Image>();
        mvSpeed = DataManager.dataInst.gameData.plSpeed;
        expImage.fillAmount = gameData.Exp / gameData.MaxExp;
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
        transform.Translate(h * Time.deltaTime * mvSpeed, 0, v * Time.deltaTime * mvSpeed, Space.World);

    }

    public void PlayerRecieveDamage(float damage)
    {
        gameData.plHP -= damage;
        HitEff();
        Camera.main.GetComponent<CameraMove>().Shake();
        if (gameData.plHP <= 0)
        {
            SceneMove.scenenInst.StartScene();
        }
        GameObject.Find("PlayerHp_Image").GetComponent<PlayerHP>().HpDown();
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
    public void PlayerExpUp()
    {
        gameData.Exp += 30f;
        expImage.fillAmount = gameData.Exp / gameData.MaxExp;
        if (gameData.Exp >= gameData.MaxExp)
        {
            GameObject.Find("SkillStore").transform.GetChild(0).GetComponent<SkillManager>().SkillStoreOpen();
            gameData.Exp -= gameData.MaxExp;
            expImage.fillAmount = gameData.Exp / gameData.MaxExp;
        }

    }
}
