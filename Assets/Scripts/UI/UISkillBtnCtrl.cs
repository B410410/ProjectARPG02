using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBtnCtrl : MonoBehaviour
{
    //public static UISkillBtnCtrl ctrl;
    
    /*void Awake()
   {
       ctrl = this;
   }*/

    private SkillData skillData;

    [Header("UI控制元件")]
    public Image skillImg;
    public Image cdImg;
    public Text cdText;
    public Text keyText;

    [Header("技能參數")]
    public KeyCode hotKey;
    private float cooldown = 1f;
    private float _timer = 0;
    private float timer
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value <= 0 ? 0 : value;
        }
    }
    private string timerStr
    {
        get
        {
            return timer > 0 ? timer.ToString("F0") : string.Empty;
        }
    }

    public bool lowMp
    {
        get
        {
            return skillData.mpCost > GameData.playerData.mp;
        }
    }

    private bool canUse
    {
        get { return timer == 0; }
    }
    private bool locked
    {
        get { return cooldown < 0; }
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetSkillData();
        GameData.KeyPressEvent += KeyChecker;
    }

    /// <summary>
    /// 設定每個技能按鍵上所屬的技能資料
    /// </summary>
    /// <param name="data">技能資料</param>
    public void SetSkillData(SkillData data)
    {
        skillData = data;
        skillImg.sprite = skillData.icon;
        cooldown = skillData.cooldown;
        string key = hotKey.ToString();
        keyText.text = key[key.Length - 1].ToString();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!canUse)
        {          
            timer -= Time.deltaTime;
            cdImg.fillAmount = timer / cooldown;
            cdText.text = timerStr;
        }
    }

    public void UpdateUI()
    {
        skillImg.color = lowMp ? Color.red : Color.white;
    }

    /// <summary>
    /// 接收按鍵訊號
    /// </summary>
    /// <param name="key">傳入值</param>
    public void KeyChecker(KeyCode key)
    {
        if (hotKey == key)
        {
            UseSkill(); 
        }
    }

    /// <summary>
    /// 使用技能(連接於介面)
    /// </summary>
    public void UseSkill()
    {
        if (lowMp) return;
        //if (skillData.cooldown < 0) return;
        if (locked || !canUse) return;
        timer = cooldown;
        GameData.SkillShoot(skillData);
    }
}
