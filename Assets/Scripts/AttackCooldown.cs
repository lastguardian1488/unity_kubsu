using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCooldown : MonoBehaviour
{
    public float delayTime = 1f;

    private float remainingTime = 0f;

    public void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime < 0f) remainingTime = 0f;
    }

    public bool isZeroed()
    {
        return remainingTime == 0f;
    }

    public void StartCooldown()
    {
        remainingTime = delayTime;
    }
}
