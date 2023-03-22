using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCtrl : CharCtrl
{
    #region 移動相關
    //模擬搖桿：操作方向向量
    private Vector2 _joystick;
    private Vector2 joystick
    {
        get
        {
            //取出橫直軸數據
            _joystick.x = Input.GetAxis("Horizontal");
            _joystick.y = Input.GetAxis("Vertical");
            return _joystick;
        }
    }
    private float rotaAng
    {
        get
        {
            //計算夾角(12點正 ~ 搖桿推向方向) * 數學庫：取正負號的方法
            return Vector2.Angle(Vector2.up, joystick) * Mathf.Sign(joystick.x)
                + GameData.cameraManager.transform.rotation.eulerAngles.y;
        }
    }
    private bool isMove
    {
        get
        {
            if (!isAction)
            {
                if (joystick != Vector2.zero)
                {//橫直軸不為0：正在執行移動
                    aniType = AniType.RUN;
                    return true;
                }
                else
                {
                    aniType = AniType.IDLE;
                }
            }
            return false;
        }
    }
    
    #endregion

    #region 攻擊相關
    private bool isAttack
    {
        get
        {
            if (actionDone && !GameData.overOnUI && Input.GetMouseButtonDown(0))
            {
                aniType = AniType.ATTACK;
                return true;
            }
            return false;
        }
    }
    private GameObject skillObj;
    #endregion


    public Transform shooter;
    public GameObject normalAttack;

    // Start is called before the first frame update
    void Start()
    {
        //通報系統：鏡頭目標(角色控制器)上場
        //GameData.SetCameraTarget(transform);
        GameData.SetPlayerCtrl(this);
        HpCtrl(hpMax);
        MpCtrl(mpMax);
    }

    [Header("升級特效")]
    public GameObject levelUpEffect;

    public override void HpCtrl(float F)
    {
        GameData.playerData.HpCtrl(F);
        GameData.UpdateUI();
    }

    public override bool MpCtrl(float F)
    {
        if (aniType == AniType.DEAD) return false;
        if (GameData.playerData.MpCtrl(F))
        {//(消耗：MP大於等於消耗量 or 補充：直接執行)
            GameData.UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void AnimaStatus()
    {
        base.AnimaStatus();
        Attack();
        Move();
    }

    /// <summary>
    /// 移動功能
    /// </summary>
    public override void Move()
    {
        if (isMove)
        {
            //角色旋轉 (四元數運算：設角度，鎖Y軸)
            transform.rotation = Quaternion.AngleAxis(rotaAng, Vector3.up);
            //角色往前 
            charCtrl.Move(transform.forward * Time.deltaTime * moveSpeed);
        }
    }

    public override void Attack()
    {
        if (isAttack)
        {
            LockAtTarget(2);//看向目標(在搜索範圍內)
            aniCtrl.SetSkillNum(0);
            skillObj = normalAttack;
        }
    }

    /// <summary>
    /// 透過技能系統給予技能資料
    /// </summary>
    /// <param name="skillNum">技能按鍵編號</param>
    /// <param name="obj">技能物件</param>
    public void SkillAttack(SkillData data)
    {
        if (!MpCtrl(-data.mpCost)) return;
        LockAtTarget();//看向目標(在搜索範圍內)

        Debug.Log(data.name);
        if (data == null) return;
        aniCtrl.SetSkillNum(data.aniNum);
        aniType = AniType.ATTACK;
        skillObj = data.obj;
        
    }

    public override void LockAtTarget(float range = 50f)
    {
        MonsterCtrl target = 
            GameData.targetSystem.SearchNear(transform.position, range);
        if (target)
        {
            transform.LookAt(target.transform);
        }
    }

    public override void Shoot()
    {
        Instantiate(skillObj, shooter.position, shooter.rotation);
    }


    public override void Dead()
    {
        aniType = AniType.DEAD;
        aniCtrl.SetAnimation(aniType);
    }

    public void LevelUp()
    {
        Instantiate(levelUpEffect, transform.position, transform.rotation);
    }
}
