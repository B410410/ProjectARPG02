using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string name;
    public string id;
    public int aniNum = 1;
    public float cooldown;
    public float mpCost;
    public Sprite icon;
    public GameObject obj;
}
