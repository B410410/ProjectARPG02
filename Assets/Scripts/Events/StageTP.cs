using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StageTP : MonoBehaviour
{
    public static bool inTP;
    private BoxCollider _box;
    private BoxCollider box
    {
        get
        {
            if (_box == null) _box = GetComponent<BoxCollider>();
            return _box;
        }
    }
    [Header("傳送目的場景")]
    public string nextStageName;

    // Start is called before the first frame update
    void Start()
    {
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider target)
    {
        if (inTP) return;
        if(target.gameObject.tag == "Player")
        {
            GameData.ChangeStage(nextStageName);
            inTP = true;
        }
    }

    private void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            inTP = false;
        }
    }
}
