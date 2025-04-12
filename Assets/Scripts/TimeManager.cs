using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject); // 防止多个实例
        }
    }

    public bool SpendTime(float seconds)
    {
        // 实现时间消耗逻辑
        Debug.Log($"Spending {seconds} seconds.");
        return true;
    }

    public void AddTime(float seconds)
    {
        // 实现时间增加逻辑
        Debug.Log($"Adding {seconds} seconds.");
    }
}