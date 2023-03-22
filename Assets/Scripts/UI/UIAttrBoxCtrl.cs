using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AttrSubUI
{
    public string keyStr;
    public Text hederText;
    public Text valText;

    public void UpdateUI()
    {
        hederText.text = keyStr;
        valText.text = GameData.SearchSubAttr(keyStr).valString;
    }
}

public class UIAttrBoxCtrl : MonoBehaviour
{
    public AttrType attrType;
    [Header("單一屬性控制元件")]
    public Text attrText;
    public Text pointText;
    public Button addBtn;
    public Button minusBtn;
    [Header("子屬性控制元件")]
    public List<AttrSubUI> subUIs;

    /// <summary>
    /// 自由點數取得
    /// </summary>
    private int freePoints
    {
        get
        {
            return GameData.playerData.freePoints;
        }
    }
    /// <summary>
    /// 欄位屬性取得
    /// </summary>
    private int attrVal
    {
        get
        {
            switch (attrType)
            {
                case AttrType.STR:
                    return GameData.playerData.attrStr;
                case AttrType.DEX:
                    return GameData.playerData.attrDex;
                case AttrType.INT:
                    return GameData.playerData.attrInt;
                default:
                    return GameData.playerData.attrCon;
            }
        }
    }
    /// <summary>
    /// 暫存點數(還未確認)
    /// </summary>
    private int tmpVal
    {
        get
        {
            switch (attrType)
            {
                case AttrType.STR:
                    return GameData.playerData.tmpStr;
                case AttrType.DEX:
                    return GameData.playerData.tmpDex;
                case AttrType.INT:
                    return GameData.playerData.tmpInt;
                default:
                    return GameData.playerData.tmpCon;
            }
        }
    }

    private string showUpVal
    {
        get
        {
            return (attrVal + tmpVal).ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        attrText.text = attrType.ToString();
        //訂閱事件：同步執行
        UIFreePointsCtrl.openEvent += UpdateUI;
        UIFreePointsCtrl.closeEvent += CloseEvent;
    }

    public void AddBtn()
    {
        GameData.AddPoint(attrType);
        UpdateUI();
    }

    public void MinusBtn()
    {
        GameData.RemovePoint(attrType);
        UpdateUI();
    }

    public void CloseEvent()
    {
        if (tmpVal > 0)
        {
            GameData.ClearPoint(attrType);
            //歸還至FreePoint
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        addBtn.interactable = freePoints > 0;
        minusBtn.interactable = tmpVal > 0;
        pointText.text = showUpVal;
        //infoText.text = GameData.playerData.meleeDamage.valString;
        foreach (AttrSubUI subUI in subUIs)
        {
            subUI.UpdateUI();
        }
    }
}
