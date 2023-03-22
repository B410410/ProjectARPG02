using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeArrow : Skill
{
    [Header("掃射點清單")]
    public List<Transform> shootPoints;

    public override void SetDamage()
    {

    }

    public override void ActionAwake()
    {
        foreach (Transform point in shootPoints)
        {
            Instantiate(skill, point.position, point.rotation);
        }
    }
}
