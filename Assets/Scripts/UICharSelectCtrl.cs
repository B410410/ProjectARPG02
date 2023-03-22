using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]//系統序列化：自定義的資料結構
public struct CharMenuGroup
{
    public CharType type;
    public Toggle toggle;
    public GameObject player;
    public Animator animator;

    public void Switch()
    {
        player.SetActive(toggle.isOn);
    }

    public void OK()
    {
        animator.Play("OK");
    }
}

public class UICharSelectCtrl : MonoBehaviour
{
    public Animator coverAnimator;
    //public List<Toggle> toggles;
    //public List<GameObject> chars;
    [Header("角色選項群組")]
    public List<CharMenuGroup> groups;

    private int selectNum;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlayBGM", 0.5f);
    }
    public void PlayBGM()
    {
        GameData.audioManager.PlayBGM("MenuBGM");
    }

    public void SelectEvent(bool B)
    {
        if (B)
        {
            GameData.audioManager.PlaySFX("MenuBtnClick");
            //Debug.Log(B);
            //起始值；終點值；增值
            for (int i = 0; i < 3; i++)
            {
                //Debug.Log(i);
                //Debug.Log(toggles[i].isOn);
                //chars[i].SetActive(toggles[i].isOn);
                //groups[i].player.SetActive(groups[i].toggle.isOn);
                if(groups[i].toggle.isOn)
                {
                    //記錄當下開著的號碼
                    selectNum = i;
                }
                groups[i].Switch();
            }
        }
    }

    public void StartPlay()
    {
        GameData.audioManager.PlaySFX("StartPlay");
        //通知系統紀錄選角資料
        GameData.charType = groups[selectNum].type;
        groups[selectNum].OK();
        coverAnimator.Play("OUT");
    }
}
