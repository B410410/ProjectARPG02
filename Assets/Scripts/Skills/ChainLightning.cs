using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Skill
{
    //被鎖定的目標清單
    public Queue<CharCtrl> list;

    public override void ActionAwake()
    {
        //和目標系統要 3個最近的目標
        list = new Queue<CharCtrl>(GameData.targetSystem.SearchNearTargets(transform.position, 30, damageCount));
    }

    public override void SetDamage()
    {
        //傷害隨著攻擊次數遞減
        damage = GameData.SearchSubAttr("MagicalDamage").val * damageCount;
        //切換目標
        target = list.Dequeue();
    }

    public override void Action()
    {
        if (damageCount > 0 && target)
        {
            //繼續運作
            transform.LookAt(target.transform);
            if (Vector3.Distance(transform.position, target.transform.position) <= 0.5f)
            {
                //HIT
                HitTarget();
                target.HpCtrl(-damage);
                //連鎖閃電運作邏輯(依照次數遞減傷害，轉換目標)
                damageCount--;
                if (damageCount == 0)
                {
                    Destroy();
                }
                else SetDamage();
            }
        }
        else
        {
            //銷毀
            Destroy();
        }
    }
}
