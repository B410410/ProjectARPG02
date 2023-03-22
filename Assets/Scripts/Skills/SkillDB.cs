using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//創立自訂義選單的修飾語法(檔名，路徑，排序)
[CreateAssetMenu(fileName = "SkillDB", menuName = "DB/SkillDB", order = 0)]
//UNITY版本的離線資料庫(存放一些既定的遊戲資料)
public class SkillDB : ScriptableObject
{
    [Header("預設資料")]
    public SkillData defaultData;

    [Header("技能清單")]
    public List<SkillData> skillDatas;

    /// <summary>
    /// 搜尋技能資料庫
    /// </summary>
    /// <param name="id">技能編碼(ID)</param>
    /// <returns>技能資料</returns>
    public SkillData SearchSkill(string id)
    {
        SkillData data = defaultData;
        //遍歷(掃描)
        foreach (SkillData d in skillDatas)
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
