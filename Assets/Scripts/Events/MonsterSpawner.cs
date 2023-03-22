using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("怪物物件")]
    public GameObject monster;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", delay);
    }

    void Spawn()
    {
        Instantiate(monster, transform.position, transform.rotation);
    }
}
