using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AniType { IDLE, RUN, ATTACK, DEAD }
public class AnimationCtrl : MonoBehaviour
{
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            return _animator;
        }
    }
    private CharCtrl _charCtrl;
    private CharCtrl charCtrl
    {
        get
        {
            if (_charCtrl == null) _charCtrl = GetComponentInParent<CharCtrl>();
            return _charCtrl;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 切換動作狀態
    /// </summary>
    /// <param name="type">狀態機</param>
    public void SetAnimation(AniType type)
    {
        //Debug.Log(type);
        animator.SetBool("Run", type == AniType.RUN);
        if (type == AniType.ATTACK) animator.SetTrigger("Attack");
        if (type == AniType.DEAD) animator.SetTrigger("Dead");
    }

    public void SetSkillNum(int skillNum)
    {
        animator.SetInteger("SkillNum", skillNum);
    }

    /// <summary>
    /// 技能觸發
    /// </summary>
    public void Shoot()
    {
        charCtrl.Shoot();
    }

    public void ActionDone()
    {
        charCtrl.ActionDone();
    }
}
