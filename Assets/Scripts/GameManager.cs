using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     public static GameManager Instance { get; private set; }

    [Header("游戏状态")]
    [SerializeField] public bool isPaused = false;
    [SerializeField] private Canvas pauseMenuCanvas; // 暂停菜单的Canvas


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

    private void Start()
    {
        pauseMenuCanvas.gameObject.SetActive(false); // 初始隐藏暂停菜单
    }

    public void Pause_game()
    {
        isPaused = true;
       // Time.timeScale = 0; // 暂停游戏时间
       // playerStatusUI.Hide(); // 隐藏状态UI
      //  pauseMenuCanvas.gameObject.SetActive(true); // 显示暂停菜单
    }

    public void Resume_game()
    {
        isPaused = false;
       // Time.timeScale = 1; // 恢复游戏时间
      //  playerStatusUI.Display(); // 显示状态UI
       // pauseMenuCanvas.gameObject.SetActive(false); // 隐藏暂停菜单
    }
}
