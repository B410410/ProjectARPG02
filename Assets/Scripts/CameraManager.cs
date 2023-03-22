using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private Transform target
    {
        get
        {
            return GameData.cameraTarget;
        }
    }
    private CinemachineVirtualCamera _cvc;
    private CinemachineVirtualCamera cvc
    {
        get
        {
            if (_cvc == null) _cvc = GetComponentInChildren<CinemachineVirtualCamera>();
            return _cvc;
        }
    }
    private CinemachineTransposer _transposer;
    private CinemachineTransposer transposer
    {
        get
        {
            if (_transposer == null) _transposer = cvc.GetCinemachineComponent<CinemachineTransposer>();
            return _transposer;
        }
    }
    #region 跟隨參數
    [Header("跟隨速度")]
    [Range(0.1f, 20f)]
    [SerializeField]
    private float _followSpeed = 3f;
    private float followSpeed
    {
        get
        {
            return Time.deltaTime * _followSpeed / disRatio;
        }
    }
    [Header("跟隨偏移")]
    [SerializeField]
    private Vector3 followOffset;
    private Vector3 defaultFollowOffset;
    private Vector3 ratioFollowOffset
    {
        get
        {
            return disRatio * defaultFollowOffset;
        }
    }
    #endregion

    #region 角度參數
    //MOUSE右鍵：點擊後的起始位置
    private Vector3 mouseStartPos;
    //MOUSE右鍵：拖曳後的起始位置
    private Vector3 mouseEndPos;
    /// <summary>
    /// 滑鼠右鍵拖曳出來的方向 (搖桿右邊香菇頭)
    /// </summary>
    private Vector3 dargVet
    {
        get
        {
            return (mouseEndPos - mouseStartPos).normalized;
        }
    }
    /// <summary>
    /// 虛擬目標的原始Angle
    /// </summary>
    private Vector3 localAng
    {
        get
        {
            return transform.rotation.eulerAngles;
        }
    }
    
    [Header("側轉速度")]
    [Range(1f, 10f)]
    [SerializeField]
    private float rotaSpeedY = 3f;
    [Header("俯仰角最大最小值")]
    [Range(60f, 85f)]
    [SerializeField]
    private float rotaAngMaxY = 80f;
    [Range(-15f, 15f)]
    [SerializeField]
    private float rotaAngMinY = 15f;
    //運算用的暫存角度
    private Vector3 ang;
    /// <summary>
    /// 虛擬目標的Rota
    /// </summary>
    private Quaternion rota
    {
        get
        {
            ang = localAng;
            //俯仰角
            ang.x += dargVet.y;
            if(ang.x <= rotaAngMinY) ang.x = rotaAngMinY;
            if(ang.x >= rotaAngMaxY) ang.x = rotaAngMaxY;
            
            //側轉角
            ang.y += dargVet.x * rotaSpeedY;
            return Quaternion.Euler(ang);
        }
    }
    #endregion

    #region 縮放參數
    private const float defaultDis = -5f;
    private const float minDis = 0.3f;
    private const float maxDis = 1.2f;
    [Header("縮放倍率")]
    [Range(minDis, maxDis)]
    [SerializeField]
    private float disRatio = 1f;
    private float zDis
    {
        get
        {
            return disRatio * defaultDis;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GameData.cameraManager = this;
        defaultFollowOffset = followOffset;
        //transposer = cvc.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Rota();
        Zoom();
    }

    void Follow()
    {
        if (target) transform.position =
                Vector3.Lerp(transform.position, target.position + ratioFollowOffset, followSpeed);
    }

    void Rota()
    {
        if (Input.GetMouseButton(1))
        {
            //MOUSE右鍵：拖曳後的起始位置
            mouseEndPos = Input.mousePosition;
            //Debug.Log(dargVet);
            transform.rotation = rota;
            mouseStartPos = Input.mousePosition;
        }
    }

    void Zoom()
    {
        //控制虛擬鏡頭參數followOffset * 倍率(限制最小~最大)
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            disRatio -= Input.mouseScrollDelta.y * 0.1f;
            disRatio = Mathf.Clamp(disRatio, minDis, maxDis);
        }
        //Debug.Log(disRatio);
        transposer.m_FollowOffset.z = Mathf.Lerp(transposer.m_FollowOffset.z, zDis, Time.deltaTime * 5f);
    }
}
