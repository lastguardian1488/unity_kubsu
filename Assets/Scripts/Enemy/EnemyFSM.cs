using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    private Action activeState; // ��������� �� �������� ��������� ��������

    public void SetState(Action state) {
        activeState = state;
    }

    public void Update()
    {
        if (activeState != null)
        {
            activeState();
        }
    }
}
