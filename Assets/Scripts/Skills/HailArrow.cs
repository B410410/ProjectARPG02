using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailArrow : Skill
{
    [Header("降雨點")]
    public List<Transform> points;
    public int spawnForPoint = 1;
    public float rangeForPoint = 1.5f;

    [Header("波次")]
    public int waves = 5;
    private int waveDone;

    [Header("延遲")]
    public float delay = 1f;
    public float delayWave = 0.5f;

    public override void SetDamage()
    {

    }

    public override void ActionAwake()
    {
        Invoke("ArrowArray", delay);
    }

    void ArrowArray()
    {
        waveDone++;
        foreach (Transform point in points)
        {
            for (int i = 0; i < spawnForPoint; i++)
            {
                Vector3 randomPos = Random.insideUnitSphere * rangeForPoint;
                //randomPos.y = 0;
                Instantiate(skill, point.position + randomPos, point.rotation);
            }
        }
        if(waveDone < waves) Invoke("ArrowArray", delayWave);
    }
}
