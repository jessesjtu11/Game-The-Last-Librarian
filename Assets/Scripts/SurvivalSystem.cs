using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    private static SurvivalSystem _instance;
    public static SurvivalSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SurvivalSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SurvivalSystem");
                    _instance = obj.AddComponent<SurvivalSystem>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject); // 防止多个实例
        }
    }

    public void AddHeat(float value)
    {
        // 实现增加热量的逻辑
    }
}