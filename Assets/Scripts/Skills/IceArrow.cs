using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : Skill
{
    private Vector3 startPos;
    private List<MonsterCtrl> list;
    public override void SetDamage()
    {
        damage = 0;
        startPos = transform.position;
        list = new List<MonsterCtrl>(GameData.targetSystem.list);
    }

    public override void Action()
    {
        if (Vector3.Distance(transform.position, startPos) < 8f)
        {
            foreach(MonsterCtrl m in list)
            {
                if (Vector3.Distance(transform.position, m.transform.position) < 2f)
                {
                    target = m;
                    break;
                }
            }
            //繼續運作
            if (target != null)
            {
                //HIT
                HitTarget();
                Instantiate(skill, transform.position, transform.rotation);
                list.Remove((MonsterCtrl)target);
                target = null;
            }
        }
        else
        {
            //銷毀
            Instantiate(skill, transform.position, transform.rotation);
            Destroy();
        }
    }
}
