using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundData
{
    public string id;
    public AudioClip clip;
}

//創立自訂義選單的修飾語法(檔名，路徑，排序)
[CreateAssetMenu(fileName = "SoundDB", menuName = "DB/SoundDB", order = 2)]
public class SoundDB : ScriptableObject
{
    [Header("音樂清單")]
    public List<SoundData> bgmDatas;

    [Header("音效清單")]
    public List<SoundData> sfxDatas;

    public AudioClip SearchBGM(string id)
    {
        AudioClip clip = null;
        //遍歷(掃描)
        foreach (SoundData d in bgmDatas)
        {
            if (d.id == id)
            {
                clip = d.clip;
                break;
            }
        }

        return clip;
    }

    public AudioClip SearchSFX(string id)
    {
        AudioClip clip = null;
        //遍歷(掃描)
        foreach (SoundData d in sfxDatas)
        {
            if (d.id == id)
            {
                clip = d.clip;
                break;
            }
        }

        return clip;
    }
}
