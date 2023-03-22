using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropData
{
    public List<DropItem> dropList;
}

[System.Serializable]
public class DropItem
{
    public string id;
    public float rate;
}

