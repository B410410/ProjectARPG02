using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 資料結構(受主屬影響的附加屬性)
/// </summary>
public struct AttrSubInfos
{
    public string header;
    public int val;
    public int tmpVal;
    public string valDesc;
    public string valString
    {
        get { return val.ToString() + (tmpVal > 0 ? $"(+{tmpVal})" : "") + valDesc; }
    }
}

public class PlayerData
{
    /// <summary>
    /// 職業
    /// </summary>
    public CharType type;
    /// <summary>
    /// 人物等級數據
    /// </summary>
    public int level = 1;

    #region 角色狀態
    public bool isdead
    {
        get
        {
            return hp == 0;
        }
    }
    #endregion 

    #region EXP設置
    public const float expBaseVal = 100f;
    /// <summary>
    /// 經驗值總量(累計)
    /// </summary>
    public float expTotal;
    /// <summary>
    /// 當前等級的經驗值分子
    /// </summary>
    public float exp
    {
        get
        {
            float point = 0;
            //起始值；終點值；增值 (算出當級以前的經驗總量)
            for (int i = 1; i < level; i++)
            {
                point += expBaseVal * i * i;
            }
            return expTotal - point;
        }
    }
    /// <summary>
    /// 當前等級的經驗值分母
    /// </summary>
    public float expMax
    {
        get
        {
            return level * level * expBaseVal;
        }
    }
    /// <summary>
    /// 當前等級的經驗值百分比
    /// </summary>
    public float expPercent
    {
        get
        {
            return exp / expMax; 
        }
    }

    /// <summary>
    /// 增加經驗值
    /// </summary>
    /// <param name="addExp">增加量</param>
    /// <returns>該次獲取經驗是否升級</returns>
    public bool AddExp(float addExp)
    {
        bool levelUp = false;
        expTotal += addExp;
        //升級驗證
        while (exp >= expMax)
        {
            level++;
            levelUp = true;
            hp = hpMax;
            mp = mpMax;
            freePoints += 5;
        }
        return levelUp;
    }
    

    #endregion

    #region HealthPoint設置
    /// <summary>
    /// 職業生命加權
    /// </summary>
    public float hpWeight
    {
        get
        {
            if (type == CharType.Warrior) return 1.3f;
            if (type == CharType.Archer) return 1f;
            if (type == CharType.Mage ) return 0.8f;
            return 1f;
        }
    }
    /// <summary>
    /// HP的基礎數值
    /// </summary>
    public const float hpBaseVal = 100f;
    /// <summary>
    /// 當前持有生命點數
    /// </summary>
    public float hp;
    /// <summary>
    /// HP的最大值 (基礎HP值 * 職業生命加權 * 等級)
    /// </summary>
    public float hpMax
    {
        get
        {
            return hpBaseVal * hpWeight * level;
        }
    }
    /// <summary>
    /// 血量百分比(當前血量 / 血量最大值)
    /// </summary>
    public float hpFillAmount
    {
        get
        {
            return hp / hpMax;
        }
    }
    /// <summary>
    /// 血量(文字)
    /// </summary>
    public string hpInfo
    {
        get
        {
            return hp.ToString() + " / " + hpMax.ToString();
        }
    }
    #endregion

    #region ManaPoint設置
    public float mpWeight
    {
        get
        {
            if (type == CharType.Warrior)
            {
                return 0.3f;
            }
            else if (type == CharType.Archer)
            {
                return 0.8f;
            }
            else
            {
                return 2.0f;
            }
        }
    }
    /// <summary>
    /// MP的基礎數值
    /// </summary>
    public const float mpBaseVal = 30f;
    /// <summary>
    /// 當前持有魔力點數
    /// </summary>
    public float mp;
    /// <summary>
    /// MP的最大值 (基礎MP值 * 職業加權 * 等級)
    /// </summary>
    public float mpMax
    {
        get
        {
            return mpBaseVal * level * mpWeight;
        }
        //set;
    }
    public float mpFillAmount
    {
        get
        {
            return mp / mpMax;
        }
    }
    public string mpInfo
    {
        get
        {
            return mp.ToString() + " / " + mpMax.ToString();
        }
    }
    #endregion

    #region 屬性點數
    public int freePoints;
    public int attrStr = 5;
    public int tmpStr;
    public AttrSubInfos meleeDamage;
    public AttrSubInfos MeleeDamage()
    {
        meleeDamage.header = "MeleeDamage";
        meleeDamage.val = 20 + (attrStr * 3);
        meleeDamage.tmpVal = tmpStr * 2;
        return meleeDamage;
    }


    public int attrDex = 5;
    public int tmpDex;
    public AttrSubInfos rangedDamage;
    public AttrSubInfos RangedDamage()
    {
        rangedDamage.header = "RangedDamage";
        rangedDamage.val = 3 + (attrDex * 6);
        rangedDamage.tmpVal = tmpDex * 6;
        return rangedDamage;
    }
    public int attrInt = 5;
    public int tmpInt;
    public AttrSubInfos magicalDamage;
    public AttrSubInfos MagicalDamage()
    {
        magicalDamage.header = "MagicalDamage";
        magicalDamage.val = attrInt * 8;
        magicalDamage.tmpVal = tmpInt * 8;
        return magicalDamage;
    }
    public int attrCon = 5;
    public int tmpCon;
    public AttrSubInfos hpRecovery;
    public AttrSubInfos HpRecovery()
    {
        hpRecovery.header = "HpRecovery";
        hpRecovery.val = 1 + (attrCon * 1);
        hpRecovery.tmpVal = tmpCon * 1;
        hpRecovery.valDesc = "/15s";
        return hpRecovery;
    }
    public bool canSave
    {
        get
        {
            return tmpStr > 0 || tmpDex > 0 || tmpInt > 0 || tmpCon > 0;
        }
    }

    public void AddPoint(AttrType attrType)
    {
        freePoints--;
        if (attrType == AttrType.STR) tmpStr++;
        if (attrType == AttrType.DEX) tmpDex++;
        if (attrType == AttrType.INT) tmpInt++;
        if (attrType == AttrType.CON) tmpCon++;
    }

    public void RemovePoint(AttrType attrType)
    {
        freePoints++;
        if (attrType == AttrType.STR) tmpStr--;
        if (attrType == AttrType.DEX) tmpDex--;
        if (attrType == AttrType.INT) tmpInt--;
        if (attrType == AttrType.CON) tmpCon--;
    }

    public void UsePoints()
    {
        attrStr += tmpStr;
        tmpStr = 0;

        attrDex += tmpDex;
        tmpDex = 0;

        attrInt += tmpInt;
        tmpInt = 0;

        attrCon += tmpCon;
        tmpCon = 0;
    }

    public void ClearPoint(AttrType attrType)
    {
        int points = 0;
        if (attrType == AttrType.STR)
        {
            points = tmpStr;
            tmpStr = 0;
        }
        if (attrType == AttrType.DEX)
        {
            points = tmpDex;
            tmpDex = 0;
        }
        if (attrType == AttrType.INT)
        {
            points = tmpInt;
            tmpInt = 0;
        }
        if (attrType == AttrType.CON)
        {
            points = tmpCon;
            tmpCon = 0;
        }
        freePoints += points;
    }
    #endregion

    #region 附加屬性(子屬性)
    public Dictionary<string, Func<AttrSubInfos>> attrSubVals 
        = new Dictionary<string, Func<AttrSubInfos>>();
    #endregion

    #region 物品金錢資訊
    public int money;
    #endregion

    #region 技能清單
    public string[] skillIDs = new string[4];
    #endregion

    #region 建構式
    /// <summary>
    /// 建構式方法 (初始化)
    /// </summary>
    /*public PlayerData()
    {
        hp = hpMax;
        mp = mpMax;
    }*/
    /// <summary>
    /// 建構式方法 (依照職業類別初始化)
    /// </summary>
    /// <param name="charType">職業類別</param>
    public PlayerData(CharType charType )
{
    type = charType;
    hp = hpMax;
    mp = mpMax;
    switch (charType)
    {
        case CharType.Warrior:
            attrStr = 10;
            attrDex = 2;
            attrInt = 0;
            attrCon = 8;
            skillIDs[0] = "WS0000";
            skillIDs[1] = "WS0001";
            skillIDs[2] = "WS0002";
            skillIDs[3] = "WS0003";
            break;

        case CharType.Archer:
            attrStr = 1;
            attrDex = 14;
            attrInt = 2;
            attrCon = 3;
            skillIDs[0] = "AS0000";
            skillIDs[1] = "AS0001";
            skillIDs[2] = "AS0002";
            skillIDs[3] = "AS0003";
            break;

        case CharType.Mage:
            attrStr = 1;
            attrDex = 2;
            attrInt = 12;
            attrCon = 5;
            skillIDs[0] = "MS0000";
            skillIDs[1] = "MS0001";
            skillIDs[2] = "MS0002";
            skillIDs[3] = "MS0003";
            break;
    }
        attrSubVals.Add("MeleeDamage", MeleeDamage);
        attrSubVals.Add("RangedDamage", RangedDamage);
        attrSubVals.Add("MagicalDamage", MagicalDamage);
        attrSubVals.Add("HpRecovery", HpRecovery);
}
    /// <summary>
    /// 建構式方法 (依照職業類別+設定等級初始化)
    /// </summary>
    /// <param name="charType">職業類別</param>
    /// <param name="lv">等級</param>
    public PlayerData(CharType charType, int lv)
    {
        type = charType;
        level = lv;
        hp = hpMax;
        mp = mpMax;
        switch (charType)
        {
            case CharType.Warrior:
                attrStr = 10;
                attrDex = 2;
                attrInt = 0;
                attrCon = 8;
                skillIDs[0] = "WS000";
                skillIDs[1] = "WS001";
                skillIDs[2] = "WS002";
                skillIDs[3] = "WS003";
                break;

            case CharType.Archer:
                attrStr = 2;
                attrDex = 10;
                attrInt = 3;
                attrCon = 5;
                skillIDs[0] = "AS000";
                skillIDs[1] = "AS001";
                skillIDs[2] = "AS002";
                skillIDs[3] = "AS003";
                break;

            case CharType.Mage:
                attrStr = 2;
                attrDex = 3;
                attrInt = 10;
                attrCon = 5;
                skillIDs[0] = "MS000";
                skillIDs[1] = "MS001";
                skillIDs[2] = "MS002";
                skillIDs[3] = "MS003";
                break;
        }

        attrSubVals.Add("MeleeDamage", MeleeDamage);
        attrSubVals.Add("RangedDamage", RangedDamage);
        attrSubVals.Add("MagicalDamage", MagicalDamage);
        attrSubVals.Add("HpRecovery", HpRecovery);
    }
    #endregion

    #region 數值控制功能
    public void HpCtrl(float F)
    {
        hp += F;
        if (hp >= hpMax) hp = hpMax;
        if (hp <= 0) hp = 0;
    }
    public bool MpCtrl(float F)
    {
        if ((F < 0 && (mp + F) >= 0) || F >= 0)
        {//(消耗：MP大於等於消耗量 or 補充：直接執行)
            mp += F;
            if (mp >= mpMax) mp = mpMax;
            if (mp <= 0) mp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool MoneyCtrl(int M)
    {
        if ((M < 0 && (money + M) >= 0) || M >= 0)
        {//(消耗：money大於等於消耗量 or 補充：直接執行)
            money += M;
            if (money <= 0) money = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}



