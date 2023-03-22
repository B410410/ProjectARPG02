using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFreePointsCtrl : MonoBehaviour
{
    /// <summary>
    /// 屬性操作相關事件：發報端
    /// </summary>
    public static event FreePointEvent openEvent;
    public static event FreePointEvent closeEvent;

    [Header("控制元件")]
    public Text freePointText;
    public Button okBtn;
    private int freePoints
    {
        get
        {
            return GameData.playerData.freePoints;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameData.SetFreePointsUI(this);
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        freePointText.text = freePoints.ToString();
        okBtn.interactable = GameData.playerData.canSave;
        //執行該屬性相關的事件功能
        if (openEvent != null) openEvent.Invoke();
    }

    public void CloseUI()
    {
        if (closeEvent != null) closeEvent.Invoke();
    }
    public void ApplyPoints()
    {
        GameData.UsePoints();
    }
}
