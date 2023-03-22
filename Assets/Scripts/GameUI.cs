using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public bool overOnUI;
    [Header("UI控制元件")]
    #region 系統介面
    public Toggle charToggle;
    public Toggle bagToggle;
    public UISwitchCtrl optionPanel;
    #endregion
    [Header("角色UI控制元件")]
    #region 角色介面
    public Image hpBarImag;
    public Text hpText;

    public Image mpBarImag;
    public Text mpText;

    public Image lvBarImag;
    public Text lvExpText;
    #endregion
    #region 技能介面
    [Header("Skill資料庫")]
    public SkillDB DB;

    public List<UISkillBtnCtrl> skillBtns;
    #endregion
    [Header("敵方UI控制元件")]
    #region 抬頭顯示介面
    public UISwitchCtrl monsterHudPanel;
    public Image hpBarMonsterImag;
    public Text hpMonsterText;
    public TextMeshProUGUI nameText;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //GAME UI託管於系統
        GameData.SetGameUI(this);
        //加載場景
        GameData.AddStage();
    }

    public void UpdateSkillData(string[] IDs )
    {
        for (int i = 0; i < IDs.Length; i++)
        {
            skillBtns[i].SetSkillData(DB.SearchSkill(IDs[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        overOnUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            //經驗值測試
            GameData.AddExp(1000f);
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(code))
                {
                    GameData.KeyChecker(code);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C)) charToggle.isOn = !charToggle.isOn;
        if (Input.GetKeyDown(KeyCode.B)) bagToggle.isOn = !bagToggle.isOn;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.Switch();
            charToggle.isOn = false;
            bagToggle.isOn = false;
        }
    }

    /// <summary>
    /// 更新下方玩家資訊UI
    /// </summary>
    public void UpdatePlayerInfoUI()
    {
        hpBarImag.fillAmount = GameData.hpFillAmount;
        mpBarImag.fillAmount = GameData.mpFillAmount;
        hpText.text = GameData.hpInfo;
        mpText.text = GameData.mpInfo;
        foreach(UISkillBtnCtrl btn in skillBtns)
        {
            btn.UpdateUI();
        }
    }

    /// <summary>
    /// 更新等級&經驗值UI
    /// </summary>
    public void UpdateExpUI()
    {
        lvBarImag.fillAmount = GameData.expPercent;
        lvExpText.text = GameData.lvExpInfos;
    }


    /// <summary>
    /// 更新上方敵對角色抬頭顯示UI
    /// </summary>
    public void UpdateHudUI()
    {
        monsterHudPanel.Switch(GameData.hpMonsterFillAmount > 0);
        hpBarMonsterImag.fillAmount = GameData.hpMonsterFillAmount;
        hpMonsterText.text = GameData.hpMonsterInfo;
        nameText.text = GameData.nameMonster;
    }
}
