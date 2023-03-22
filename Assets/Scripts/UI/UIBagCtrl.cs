using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagCtrl : MonoBehaviour
{
    [Header("金幣UI")]
    public Text moneyText;

    [Header("背包格模板")]
    public ItemDB itemDB;
    public UISlotsCtrl slotsTmp;
    public RectTransform rtGroup;

    [Header("背包列數")]
    public int rowCount = 12;
    private int slotsCount
    {
        get
        {
            return 4 * rowCount;
        }
    }

    private List<UISlotsCtrl> slotsList = new List<UISlotsCtrl>();

    // Start is called before the first frame update
    void Start()
    {
        GameData.SetBagUI(this);
        CreateSlots();
    }

    /// <summary>
    /// 建立背包格
    /// </summary>
    void CreateSlots()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            slotsList.Add(Instantiate(slotsTmp, rtGroup).SetDefault(i));
        }
    }

    public void BagStatus(bool isOpen)
    {
        if (isOpen)
        {
            GameData.bagIsOpen = true;
            UpdateUI();
        }
        else
        {
            GameData.bagIsOpen = false;
        }
    }
    public void UpdateMoneyUI()
    {
        moneyText.text = GameData.playerData.money.ToString();
    }

    public void UpdateUI()
    {
        //更新金幣
        UpdateMoneyUI();
        //更新物品
        UpdataItemsUI();
    }
    public void UpdataItemsUI()
    {
        Dictionary<string, int> bagData = GameData.LoadBag();
        ItemData itemTmp = null;
        int index = 0;
        foreach (KeyValuePair<string, int> data in bagData)
        {
            if (data.Value > 0)
            {
                itemTmp = itemDB.SearchItem(data.Key);
                if (itemTmp != null) index = SetSlotsCtrl(index, itemTmp, data.Value);
            }
        }
        for(int i = index ; i < slotsList.Count; i++)
        {
            slotsList[i].Clear();
        }
    }
    /// <summary>
    /// 背包資料置入控制
    /// </summary>
    /// <param name="start">起始欄位</param>
    /// <param name="data">物品格式</param>
    /// <param name="count">物品總數</param>
    /// <returns>最後停留的空格</returns>
    int SetSlotsCtrl(int start, ItemData data, int count)
    {
        //填滿的格數
        int slotsFillUp = count / data.stackCount;
        //未滿一格的殘數
        int slotsRemainder = count % data.stackCount;
        //起始格號 + 總使用格數
        int slotsCount = start + slotsFillUp + (slotsRemainder > 0 ? 1 : 0);

        for (int i = start; i < slotsCount; i++)
        {
            slotsList[i].SetData(data, i < start + slotsFillUp ? data.stackCount : slotsRemainder);
        }
        return slotsCount;
    }
}
