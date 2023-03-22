using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [Header("關卡音樂清單")]
    public string bgmID;

    [Header("角色清單")]
    public List<CharCtrl> chars;

    // Start is called before the first frame update
    void Start()
    {
        GameData.audioManager.PlayBGM(bgmID);
        Debug.Log(GameData.charType);
        if(GameData.playerCtrl == null)
        {
            //具現化角色物件
            Instantiate(chars[GameData.GetPlayerData()], transform.position, transform.rotation);
        }
        else
        {
            if (GameData.CheckStagePos() == Vector3.one * -1)
            {
                GameData.playerCtrl.transform.position = transform.position;
            }
            else
            {
                GameData.playerCtrl.transform.position = GameData.CheckStagePos();
            }
        }
        //StageTP.inTP = false;
    }

}
