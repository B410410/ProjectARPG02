using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 按鍵偵測的委派格式
/// </summary>
/// <param name="key">按鍵值</param>
public delegate void KeyChecker(KeyCode key);

public static class GameData
{
    #region 角色管理相關
    /// <summary>
    /// 紀錄選取的角色類別
    /// </summary>
    public static CharType charType = CharType.Mage;
    /// <summary>
    /// 資料實體位置
    /// </summary>
    private static PlayerData _playerData;
    /// <summary>
    /// 對外公開的接口
    /// </summary>
    public static PlayerData playerData
    {
        get
        {
            if(_playerData == null) _playerData = new PlayerData(charType);
            return _playerData;
        }
    }

    public static float expPercent
    {
        get
        {
            return playerData.expPercent;
        }
    }

    public static string lvExpInfos
    {
        get
        {
            return $"LV.{playerData.level}  {playerData.exp} / {playerData.expMax}"+
                $"({playerData.expPercent * 100f}%)";
        }
    }

    /// <summary>
    /// 獲得EXP功能
    /// </summary>
    /// <param name="addExp">獲得EXP的值</param>
    public static void AddExp(float addExp)
    {
        if (playerData.AddExp(addExp))
        {
            //升級事件
            playerCtrl.LevelUp();
            gameUI.UpdatePlayerInfoUI();
            freePointsCtrl.UpdateUI();
        }
        gameUI.UpdateExpUI();
    }


    /// <summary>
    /// 返回角色類型的數據
    /// </summary>
    /// <returns>列舉轉為數字</returns>
    public static int GetPlayerData()
    {
        //順便產生角色的資料
        //playerData = new PlayerData(charType);
        return (int)charType;
    }

    public static CameraManager cameraManager;
    //鏡頭目標實體資料
    private static Transform _cameraTarget;
    /// <summary>
    /// 鏡頭目標公開接口
    /// </summary>
    public static Transform cameraTarget
    {
        get
        {
            return _cameraTarget;
        }
        private set
        {
            _cameraTarget = value;
        }
    }
    /// <summary>
    /// 設定鏡頭目標
    /// </summary>
    /// <param name="target">目標物</param>
    public static void SetCameraTarget(Transform target)
    {
        cameraTarget = target;
    }
    
    /// <summary>
    /// 角色控制器
    /// </summary>
    private static PlayerCtrl _playerCtrl;
    public static PlayerCtrl playerCtrl
    {
        get { return _playerCtrl; }
        private set { _playerCtrl = value; }
    }
    /// <summary>
    /// 設定角色控制器
    /// </summary>
    /// <param name="ctrl">控制器</param>
    public static void SetPlayerCtrl(PlayerCtrl ctrl)
    {
        playerCtrl = ctrl;
        SetCameraTarget(ctrl.transform);
    }
    #endregion

    #region 介面數據相關
    private static GameUI gameUI;
    public static bool overOnUI
    {
        get
        {
            return gameUI.overOnUI;
        }
    }
    public static float hpFillAmount
    {
        get
        {
            return playerData != null ? playerData.hpFillAmount : 0;
        }
    }
    public static float mpFillAmount
    {
        get
        {
            return playerData != null ? playerData.mpFillAmount : 0;
        }
    }
    public static string hpInfo
    {
        get
        {
            return playerData != null ? playerData.hpInfo : "0 / 0";
        }
    }
    public static string mpInfo
    {
        get
        {
            return playerData != null ? playerData.mpInfo : "0 / 0";
        }
    }
    /// <summary>
    /// 遊戲介面設定託管
    /// </summary>
    /// <param name="ui">介面</param>
    public static void SetGameUI(GameUI ui)
    {
        gameUI = ui;//設定
        gameUI.UpdateSkillData(playerData.skillIDs);
        gameUI.UpdatePlayerInfoUI();//初始化
        gameUI.UpdateExpUI();//初始化
    }
    public static void UpdateUI()
    {
        if (!gameUI || !playerCtrl) return;
        gameUI.UpdatePlayerInfoUI();
    }

    private static MonsterCtrl focusMonster;
    public static float hpMonsterFillAmount
    {
        get
        {
            return focusMonster ? focusMonster.hpFillAmount : 0;
        }
    }
    public static string hpMonsterInfo
    {
        get
        {
            return focusMonster ? focusMonster.hpInfo : "0 / 0";
        }
    }
    public static string nameMonster
    {
        get
        {
            return focusMonster ? focusMonster.charName : "??";
        }
    }
    public static void SetFocusTarget(MonsterCtrl target)
    {
        focusMonster = target;
        UpdateHudUI();
    }
    public static void UpdateHudUI()
    {
        if (!gameUI) return;
        gameUI.UpdateHudUI();
    }
    #endregion

    #region 角色屬性介面相關
    private static UIFreePointsCtrl freePointsCtrl;
    public static void SetFreePointsUI(UIFreePointsCtrl ctrl)
    {
        freePointsCtrl = ctrl;
    }

    public static void AddPoint(AttrType attrType)
    {
        playerData.AddPoint(attrType);
        freePointsCtrl.UpdateUI();
    }
    public static void RemovePoint(AttrType attrType)
    {
        playerData.RemovePoint(attrType);
        freePointsCtrl.UpdateUI();
    }
    public static void UsePoints()
    {
        playerData.UsePoints();
        freePointsCtrl.UpdateUI();
    }
    public static void ClearPoint(AttrType attrType)
    {
        playerData.ClearPoint(attrType);
        freePointsCtrl.UpdateUI();
    }
    /// <summary>
    /// 附加屬性查詢
    /// </summary>
    /// <param name="key">查詢關鍵字</param>
    /// <returns>附加屬性</returns>
    public static AttrSubInfos SearchSubAttr(string key)
    {
        return playerData.attrSubVals[key].Invoke();
    }
    #endregion

    #region 技能管理

    public static event KeyChecker KeyPressEvent;
    public static void KeyChecker(KeyCode keyCode)
    {
        //Debug.Log(keyCode);
        if (KeyPressEvent != null)
        {
            KeyPressEvent(keyCode);
        }
    }
    public static void SkillShoot(SkillData data)
    {
        playerCtrl.SkillAttack(data);
    }
    #endregion

    #region 場景管理相關
    public static string currentStage = "Stage1";
    public static Dictionary<string, Vector3> stagePosRecord = new Dictionary<string, Vector3>();
    public static Vector3 CheckStagePos()
    {
        if (stagePosRecord.ContainsKey(currentStage))
        {
            return stagePosRecord[currentStage];
        }
        return Vector3.one * -1;
    }
    public static void AddStage()
    {
        //使用加載模式讀取場景 LoadSceneMode.Additive
        SceneManager.LoadScene(currentStage, LoadSceneMode.Additive);
    }
    public static void ChangeStage(string stageName)
    {
        if (stagePosRecord.ContainsKey(currentStage))
        {
            stagePosRecord[currentStage] = playerCtrl.transform.position;
        }
        else
        {
            stagePosRecord.Add(currentStage, playerCtrl.transform.position);
        }
        
        targetSystem.ClearTargets();
        SceneManager.UnloadSceneAsync(currentStage, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        currentStage = stageName;
        AddStage();
    }
    #endregion

    #region 目標系統
    private static TargetSystem _targetSystem;
    public static TargetSystem targetSystem
    {
        get
        {
            if (_targetSystem == null) _targetSystem = new TargetSystem();
            return _targetSystem;
        }
    }
    #endregion

    #region 物品管理相關
    private static UIBagCtrl bagUI;
    public static void SetBagUI(UIBagCtrl ui)
    {
        bagUI = ui;
    }
    public static void SaveMoney(int money)
    {
        playerData.MoneyCtrl(money);
        if (bagIsOpen) bagUI.UpdateMoneyUI();
    }

    /// <summary>
    /// 物品欄(Bag)是否開啟
    /// </summary>
    public static bool bagIsOpen;
    private static Dictionary<string, int> bagData = new Dictionary<string, int>();
    /// <summary>
    /// 儲存物品
    /// </summary>
    /// <param name="itemID">物品ID</param>
    /// <param name="count">數量</param>
    public static void SaveBag(string itemID, int count = 1)
    {
        if (!bagData.ContainsKey(itemID))
        {
            bagData.Add(itemID, count);
        }
        else
        {
            bagData[itemID] += count;
        }
        if (bagIsOpen) bagUI.UpdataItemsUI();
    }
    public static Dictionary<string, int> LoadBag()
    {
        return bagData;
    }
    /// <summary>
    /// 依照機率表取得掉落物
    /// </summary>
    /// <param name="dropData">機率資料</param>
    /// <returns>掉落物ID</returns>
    public static string GetDropItemID(DropData dropData)
    {
        string itemID = "null";
        float val = Random.Range(0f, 1f);

        for (int i = 0; i < dropData.dropList.Count; i++)
        {
            if (val < dropData.dropList[i].rate)
            {//中獎->停止
                itemID = dropData.dropList[i].id;
                break;
            }
            else
            {//沒中->重設起點
                val -= dropData.dropList[i].rate;
            }
        }
        return itemID;
    }
    public static void UseItem(string id)
    {
        ItemEffect.GetEffect(id);
    }
    #endregion

    private static AudioManager _audioManager;
    public static AudioManager audioManager
    {
        get { return _audioManager; }
        private set { _audioManager = value; }
    }

    public static void SetAudioManager(AudioManager manager)
    {
        _audioManager = manager;
    }
}

public static class ItemEffect
{
    public static void GetEffect(string id)
    {
        switch (id)
        {
            case "IEF0000":
                SmallHeal();
                break;
            case "IEF0001":
                SmallManaUp();
                break;
        }
    }

    public static void SmallHeal()
    {
        GameData.playerData.HpCtrl(80);
        GameData.UpdateUI();
    }

    public static void SmallManaUp()
    {
        GameData.playerData.MpCtrl(50);
        GameData.UpdateUI();
    }
}

public class TargetSystem
{
    /// <summary>
    /// 目標清單
    /// </summary>
    private List<MonsterCtrl> targets = new List<MonsterCtrl>();
    public List<MonsterCtrl> list
    {
        get
        {
            return targets;
        }
    }

    /// <summary>
    /// 加入目標清單
    /// </summary>
    /// <param name="target">目標</param>
    public void AddTarget(MonsterCtrl target)
    {
        targets.Add(target);
    }

    /// <summary>
    /// 移除目標清單
    /// </summary>
    /// <param name="target">目標</param>
    public void RemoveTarget(MonsterCtrl target)
    {
        targets.Remove(target);
    }

    public void ClearTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].Destory();
        }
        targets.Clear();
    }

    /// <summary>
    /// 搜索最近目標物(預設：50)
    /// </summary>
    /// <param name="center">中心點</param>
    /// <returns>目標</returns>
    public MonsterCtrl SearchNear(Vector3 center, float defaultRange = 50f)
    {
        MonsterCtrl monster = null;
        float range = defaultRange;//預設搜索範圍
        foreach (MonsterCtrl m in targets)
        {
            float dis = Vector3.Distance(center, m.transform.position);
            if (dis < range)//在範圍內
            {
                monster = m;//鎖定
                range = dis;//縮短搜索範圍
            }
        }
        GameData.SetFocusTarget(monster);//系統回報
        return monster;
    }

    public List<MonsterCtrl> SearchNearTargets(Vector3 center, float defaultRange = 50f, int count = 1)
    {
        List<MonsterCtrl> list = new List<MonsterCtrl>();

        return targets;
    }
}
