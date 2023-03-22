using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//綁定必須的腳本
[RequireComponent(typeof(CharacterController))]
public abstract class CharCtrl : MonoBehaviour
{
    #region 元件
    //實體存放角色控制元件的資料欄位
    private CharacterController _charCtrl;
    //對外公開讀取用的接口(唯讀)
    public CharacterController charCtrl
    {
        get
        {
            //自動確認是否抓取過物件，自動抓取
            if (_charCtrl == null) _charCtrl = GetComponent<CharacterController>();
            return _charCtrl;
        }
    }
    private AnimationCtrl _aniCtrl;
    //對外公開讀取用的接口(唯讀)
    public AnimationCtrl aniCtrl
    {
        get
        {
            //自動確認是否抓取過物件，自動抓取
            if (_aniCtrl == null) _aniCtrl = GetComponentInChildren<AnimationCtrl>();
            return _aniCtrl;
        }
    }
    #endregion

    #region 角色數值
    [Header("移動速度")]
    public float moveSpeed = 3f;

    [Header("基本數值")]
    public string charName = "Monster";
    public float hpMax = 100;
    protected float hp;
    public float hpFillAmount
    {
        get { return hp / hpMax; }
    }
    public string hpInfo
    {
        get { return hp.ToString("F0") + " / " + hpMax.ToString("F0"); }
    }
    public float mpMax = 30;
    protected float mp;
    public float mpFillAmount
    {
        get { return mp / mpMax; }
    }
    public string mpInfo
    {
        get { return mp.ToString("F0") + " / " + mpMax.ToString("F0"); }
    }
    #endregion

    #region 角色狀態
    protected AniType aniType;
    protected bool isAction
    {
        get
        {
            return aniType != AniType.IDLE && aniType != AniType.RUN;
        }
    }
    /// <summary>
    /// 動作是否完成
    /// </summary>
    protected bool actionDone = true;
    #endregion



    // Update is called once per frame
    void Update()
    {
        if (aniType == AniType.DEAD) return;

        AnimaStatus();
    }

    public virtual void HpCtrl(float F)
    {
        if (aniType == AniType.DEAD) return;

        hp += F;
        if (hp >= hpMax) hp = hpMax;
        if (hp <= 0)
        {
            hp = 0;
            Dead();
        }
    }

    public virtual bool MpCtrl(float F)
    {
        if (aniType == AniType.DEAD) return false;
        if ((F < 0 && (mp + F) >= 0) || F >= 0)
        {//(消耗：MP大於等於消耗量 or 補充：直接執行)
            mp += F;
            if (mp >= mpMax) mp = mpMax;
            if (mp <= 0) mp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void AnimaStatus()
    {
        if (actionDone)
        {
            aniCtrl.SetAnimation(aniType);
            //動作開始執行：且未完成
            if (isAction) actionDone = false;
        }
    }
    public abstract void Move();
    public abstract void Attack();
    public abstract void LockAtTarget(float range = 50f);
    public abstract void Dead();

    public abstract void Shoot();

    /// <summary>
    /// 動作檔完成的設定(和動畫事件連結)
    /// </summary>
    public void ActionDone()
    {
        aniType = AniType.IDLE;
        actionDone = true;
    }

    public void Destory()
    {
        Destroy(gameObject);
    }
}
