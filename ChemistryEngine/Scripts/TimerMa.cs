using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMa : MonoBehaviour
{
    #region 单例
    private static TimerMa instance;
    public static bool StartFlag = false;
    TimerMa() { }

    public static TimerMa I
    {
        get
        {
            if (instance == null)
                instance = new TimerMa();
            return instance;
        }
    }

    #endregion
    private List<Timer> timerList;

    private void Awake()
    {
        instance = this;
        timerList = new List<Timer>();
    }

    public void AddTimer(Timer t)
    {
        timerList.Add(t);
    }

    void Update()
    {
        // if (StartFlag)
            for (int i = 0; i < timerList.Count;)
            {
                timerList[i].Run();
                // Debug.Log("timerma");
                //计时结束,且需要销毁
                if (!timerList[i].isActive && timerList[i].isDestroy)
                {
                    timerList.RemoveAt(i);
                    // StartFlag = false;
                }
                else
                    ++i;
            }
    }
}