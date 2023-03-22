using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    public override void SetDamage()
    {
        damage = GameData.SearchSubAttr("MagicalDamage").val;
    }
}
