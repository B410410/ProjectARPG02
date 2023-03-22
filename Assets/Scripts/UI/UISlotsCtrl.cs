using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotsCtrl : MonoBehaviour
{
    public static UISlotsCtrl selectSlots;
    public Sprite emptyIcon;
    public Image itemImg;
    public Text countText;
    private ItemData itemData;

    /// <summary>
    /// 初始化背包格
    /// </summary>
    /// <param name="index">格子索引編號</param>
    public UISlotsCtrl SetDefault(int index)
    {
        //把格子變成可用狀態
        gameObject.SetActive(true);
        gameObject.name = index.ToString("00") + "-Slots";
        countText.text = string.Empty;
        return this;
    }

    public void SetData(ItemData data, int count = 1)
    {
        itemData = data;
        itemImg.sprite = itemData.icon;
        countText.text = count > 1 ? count.ToString() : string.Empty;
    }

    public void Clear()
    {
        itemData = null;
        itemImg.sprite = emptyIcon;
        countText.text = string.Empty;
    }
    public void Select()
    {
        if(selectSlots != this)
        {
            selectSlots = this;
            Debug.Log("選取物品" + itemData.name);
        }
        else
        {
            use();
        }
    }

    void use()
    {
        Debug.Log("使用物品：" + itemData.name);
        GameData.UseItem(itemData.effectId);
        GameData.SaveBag(itemData.id, -1);
    }
}
