using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("附屬技能物件")]
    public Skill skill;

    [Header("特效")]
    public GameObject desEffect;
    public GameObject hitEffect;

    [Header("傷害類型")]
    [Tooltip("傷害次數")]
    public int damageCount = 1;
    protected float damage = 1;

    /// <summary>
    /// 是否執行初始行為
    /// </summary>
    [Tooltip("是否有起始行為")]
    public bool instant;

    [Header("飛射速度")]
    public float flySpeed = 0;

    /// <summary>
    /// 鎖定目標對象
    /// </summary>
    protected CharCtrl target;

    // Start is called before the first frame update
    void Start()
    {
        if (instant) ActionAwake();
        SetDamage();
    }

    // Update is called once per frame
    void Update()
    {
        Fly();
        Action();
    }

    public abstract void SetDamage();
    public virtual void ActionAwake()
    {
        Debug.LogError("未執行任何複寫內容");
    }

    public virtual void Action()
    {
        //Debug.LogError("未執行任何複寫內容");
    }

    public virtual void Fly()
    {
        if (flySpeed > 0) transform.Translate(Vector3.forward * Time.deltaTime * flySpeed);
    }

    public virtual void HitTarget()
    {
        if (hitEffect) Instantiate(hitEffect, transform.position, transform.rotation);
    }

    public virtual void Destroy()
    {
        if (desEffect) Instantiate(desEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    #region 物理型式的打擊
    public virtual void HitAction(GameObject target)
    {
        //Debug.Log(target.name);
        if (target.tag == "Wall" || damageCount <= 0)
        {
            //防止無特效時發生錯誤
            Destroy();
        }

        //Debug.Log(target.name);
        if (target.tag == "Ground" || damageCount <= 0)
        {
            //防止無特效時發生錯誤
            Destroy();
        }

        if (target.tag == "Monster")
        {
            //防止無特效時發生錯誤
            HitTarget();
            //Destroy(target);
            MonsterCtrl monster = target.GetComponent<MonsterCtrl>();
            if (monster) monster.HpCtrl(-damage);
            damageCount--;
            if (damageCount <= 0) Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        //Debug.Log(target.name);
        HitAction(target.gameObject);
    }
    #endregion

}
