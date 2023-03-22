using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//創立自訂義選單的修飾語法(檔名，路徑，排序)
[CreateAssetMenu(fileName = "ItemDB", menuName = "DB/ItemDB", order = 1)]
public class ItemDB : ScriptableObject
{
    [Header("物品清單")]
    public List<ItemData> itemDatas;

    /// <summary>
    /// 搜尋物品資料庫
    /// </summary>
    /// <param name="id">物品編碼(ID)</param>
    /// <returns>物品資料</returns>
    public ItemData SearchItem(string id)
    {
        ItemData data = null;
        //遍歷(掃描)
        foreach (ItemData d in itemDatas)
        {
            if (d.id == id)
            {
                data = d;
                break;
            }
        }

        return data;
    }
}


