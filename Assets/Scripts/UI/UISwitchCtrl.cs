using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UISwitchCtrl : MonoBehaviour
{
    private CanvasGroup _CG;
    public CanvasGroup CG
    {
        get
        {
            if (_CG == null) _CG = GetComponent<CanvasGroup>();
            return _CG;
        }
    }
    [Header("是否運行預設顯示")]
    public bool showAwake = false;
    [Header("面板開啟事件")]
    public UnityEvent openEvents;
    [Header("面板關閉事件")]
    public UnityEvent closeEvents;

    // Start is called before the first frame update
    void Start()
    {
        Switch(showAwake);
    }

    [ContextMenu("介面開關")]
    public void Switch()
    {
        //看的到按下=>看不到；看不到按下=>看的到
        CG.alpha = CG.blocksRaycasts ? 0 : 1;
        //相反的自己
        CG.blocksRaycasts = !CG.blocksRaycasts;
    }

    public void Switch(bool B)
    {
        CG.alpha = B ? 1 : 0;
        CG.blocksRaycasts = B;

        if (B && openEvents != null)
        {
            openEvents.Invoke();
        }
        if (!B && closeEvents != null)
        {
            closeEvents.Invoke();
        }
    }

}
