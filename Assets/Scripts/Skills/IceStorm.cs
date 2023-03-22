using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStorm : Skill
{
    public Queue<MonsterCtrl> list;
    public override void SetDamage()
    {
        damage = GameData.SearchSubAttr("MagicalDamage").val * 3f;
        list = new Queue<MonsterCtrl>(GameData.targetSystem.list);
        while (list.Count > 0)
        {
            target = list.Dequeue();
            if (Vector3.Distance(transform.position, target.transform.position) < 2f)
            {
                target.HpCtrl(-damage);
            }
        }
        Invoke("Destroy", 0.6f);
    }
}
