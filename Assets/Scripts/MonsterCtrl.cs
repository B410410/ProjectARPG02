using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : CharCtrl
{
    private PlayerCtrl target
    {
        get { return GameData.playerCtrl; }
    }

    private bool inSearchRange
    {
        get
        {
            return Vector3.Distance(transform.position, target.transform.position) <= searchRange;
        }
    }
    private bool inAttackRange
    {
        get
        {
            return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
        }
    }

    private bool isMove
    {
        get
        {
            return !inAttackRange && inSearchRange;
        }
    }
    private bool isAttack
    {
        get
        {
            return inAttackRange;
        }
    }
    [Header("經驗值")]
    public float exp = 1f;

    [Header("行為參數設定")]
    public float searchRange = 5f;
    public float attackRange = 2f;

    [Header("掉落機率表")]
    public TextAsset dropJson;
    public DropData dropData;

    [Header("掉落金幣數")]
    public int dropMoneyMin;
    public int dropMoneyMax;

    private void Awake()
    {
        //登場主動通報
        GameData.targetSystem.AddTarget(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.HpCtrl(hpMax);
        //Debug.Log(JsonUtility.ToJson(dropData));
        dropData = JsonUtility.FromJson<DropData>(dropJson.text);
    }

    public override void HpCtrl(float F)
    {
        base.HpCtrl(F);
        GameData.UpdateHudUI();
    }

    public override bool MpCtrl(float F)
    {
        bool B = base.MpCtrl(F);
        GameData.UpdateHudUI();
        return B;
    }

    public override void AnimaStatus()
    {
        base.AnimaStatus();
        if (!isAttack && !isMove) aniType = AniType.IDLE;
        if (isAttack) Attack();
        if (isMove) Move();

    }

    public override void Move()
    {
        if (actionDone)
        {
            aniType = AniType.RUN;
            transform.LookAt(target.transform);
            charCtrl.Move(transform.forward * Time.deltaTime * moveSpeed);
        }

    }

    public override void Attack()
    {
        transform.LookAt(target.transform);
        aniType = AniType.ATTACK; Debug.Log("Attack");
    }

    public override void LockAtTarget(float range = 50)
    {

    }

    public override void Shoot()
    {
        GameData.playerCtrl.HpCtrl(-1);
    }

    public override void Dead()
    {
        //獲取經驗值
        GameData.AddExp(exp);
        //獲取掉落物ID
        string itemID = GameData.GetDropItemID(dropData);
        Debug.Log("掉落：" + itemID);
        GameData.SaveBag(itemID);
        //掉錢$$
        if (dropMoneyMax > 0)
        {
            int dropMoney = dropMoneyMin == dropMoneyMax ? dropMoneyMax : Random.Range(dropMoneyMin, dropMoneyMax);
            GameData.SaveMoney(dropMoney); Debug.Log(dropMoney);
        }

        charCtrl.enabled = false;
        GameData.targetSystem.RemoveTarget(this);
        aniType = AniType.DEAD;
        aniCtrl.SetAnimation(aniType);
        Invoke("Destory", 1.5f);
    }

}
