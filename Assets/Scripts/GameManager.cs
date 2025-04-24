using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        PreGame,    // 游戏开始前
        Playing,    // 游戏进行中
        Paused,     // 暂停状态
        GameOver    // 游戏结束
    }

    [Header("游戏设置")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string firstLevelScene = "Dialogue";
    
    [Header("存档设置")]
    [SerializeField, Tooltip("自动保存间隔（秒）")] 
    private float autoSaveInterval = 300f;

    // 事件系统
    public event Action<GameState> OnGameStateChanged;
    public event Action OnGameSaved;
    public event Action OnGameLoaded;
    public event Action OnGameExiting;

    private GameState _currentState = GameState.PreGame;
    private Coroutine _autoSaveCoroutine;
    private bool _isQuitting = false;


        
    [Header("菜单UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] public GameObject pauseMenuPanel;   
    [SerializeField] private GameObject timeManagerPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI survivalDaysText;
    [SerializeField] private Button continueButton;

    [Header("过渡效果")] 
    [SerializeField] private float menuFadeDuration = 0.5f;
    [SerializeField] private float gameOverDelay = 1.5f;

    private Stack<GameObject> _activeMenuStack = new Stack<GameObject>();
    private bool _isTransitioning;



    // 当前游戏状态（公共只读属性）
    public GameState CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnGameStateChanged?.Invoke(value);
            }
        }
    }

    private void Awake()
    {
        InitialSingleton();
        SceneManager.activeSceneChanged += HandleSceneChange;
        Application.wantsToQuit += OnWantsToQuit;
    }

    private void Start()
    {
        InitMenuSystem();
        if (CurrentState == GameState.PreGame) ShowMainMenu();
        Player.Instance.OnTemperatureTooLow += HandleTemperatureTooLow;
        if (PlayerPrefs.HasKey("HasValidSave"))
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleSystemInput();
    }

    private void InitMenuSystem()
    {
        // 初始隐藏所有菜单
        SetMenuActive(mainMenuPanel, false);
        SetMenuActive(pauseMenuPanel, false);
        SetMenuActive(gameOverPanel, false);
    }

    private void ShowMainMenu()
    {
        if (CurrentState != GameState.PreGame) return;
        StartCoroutine(TransitionMenu(mainMenuPanel)); 
        if (PlayerPrefs.HasKey("DaysRecord"))
            recordText.text = "最长存活记录: " + PlayerPrefs.GetInt("DaysRecord", 0).ToString() + " 天";
    }



    #region 核心游戏流程
    public void StartNewGame()
    {
        ResetAllManagers();
        StartCoroutine(LoadGameScene(firstLevelScene, true));
        CurrentState = GameState.Playing;
        StartAutoSave();
        CloseTopMenu();
        TimeManager.Instance.timeScale = 0f;
        PlayerPrefs.SetInt("HasValidSave", 1); // 设置存档标记
    }

    public void RestartGame()
    {
        StopAutoSave();
        LoadGame();
        CurrentState = GameState.Playing;
        StartAutoSave();
        CloseTopMenu();
    }

    private void ResetAllManagers()
    {
        TimeManager.Instance.ResetToDayZero();
        Player.Instance.ResetPlayerState();
    
        // 清空存档
        PlayerPrefs.DeleteKey("LastSavedScene");
        PlayerPrefs.DeleteKey("TotalPlayTime");
    }


    public void ExitGame()
    {
        if (_isQuitting) return;
        _isQuitting = true;
        
        OnGameExiting?.Invoke();
        SaveGame();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnPauseClicked()
    {
        Debug.Log("Pause button clicked.");
        if (CurrentState != GameState.Playing) return;
        PauseGame();
        ShowPauseMenu();        
    }

    public void OnResumeClicked()
    {
        Debug.Log("Resume button clicked.");
        if (CurrentState != GameState.Paused) return;
        ResumeGame();
        while (_activeMenuStack.Count > 0)
        {
            CloseTopMenu();
        }
    }

    
    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;
    
        CurrentState = GameState.Paused;
        TimeManager.Instance.timeScale = 0f;
        AudioListener.pause = true;
        RoomManager.Instance.DisableRoomInteraction(); // 禁止房间交互
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        CurrentState = GameState.Playing;
        TimeManager.Instance.timeScale = 1f;
        AudioListener.pause = false;
        RoomManager.Instance.EnableRoomInteraction(); // 恢复房间交互
    }



    private void HandleSystemInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (CurrentState)
            {
                case GameState.Playing:
                    OnPauseClicked();
                    break;
                case GameState.Paused when _activeMenuStack.Count == 1:
                    OnResumeClicked();
                    break;
                default:
                    CloseTopMenu();
                    break;
            }
        }
    }
    #endregion




    #region 存档系统
    public void SaveGame()
    {
        // 保存核心数据
        PlayerPrefs.SetString("LastSavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("TotalPlayTime", (int)Time.realtimeSinceStartup);
        
        // 通知其他系统保存数据
        TimeManager.Instance.SaveTimeData();
        Player.Instance.SavePlayerState();
        
        PlayerPrefs.Save();
        OnGameSaved?.Invoke();
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameScene(
            PlayerPrefs.GetString("LastSavedScene", firstLevelScene), 
            loadSaveData: true
        ));
    }

    private void StartAutoSave()
    {
        StopAutoSave();
        _autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            SaveGame();
        }
    }

    private void StopAutoSave()
    {
        if (_autoSaveCoroutine != null)
        {
            StopCoroutine(_autoSaveCoroutine);
            _autoSaveCoroutine = null;
        }
    }
    #endregion

    #region 场景管理
    public IEnumerator LoadGameScene(string sceneName, bool loadSaveData)
    {
        // 显示加载界面
        yield return ScreenFader.Instance.FadeOut(1f);
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
        {
            // 更新加载进度条
            yield return null;
        }

        if (loadSaveData)
        {
            LoadGameData();
        }
        Debug.Log("Game loaded successfully.");

        if (sceneName == "Game"){
            timeManagerPanel.SetActive(true);
            Debug.Log(Player.Instance.currentRoomIndex);
            RoomManager.Instance.allRooms[Player.Instance.currentRoomIndex - 1].gameObject.SetActive(true); //激活当前房间
            TimeManager.Instance.timeScale = 1f; // 恢复时间流逝
        }

        yield return ScreenFader.Instance.FadeIn(1f);
    }

    private void LoadGameData()
    {
        TimeManager.Instance.LoadTimeData();
        Player.Instance.LoadPlayerState();
        OnGameLoaded?.Invoke();
    }
    #endregion

    #region GameOver逻辑
    private void HandleTemperatureTooLow()
    {
        StartCoroutine(TriggerGameOver());        
    }

    private IEnumerator TriggerGameOver()
    {
        CurrentState = GameState.GameOver;
        yield return new WaitForSecondsRealtime(gameOverDelay);
       
        Time.timeScale = 0f;        
        SetMenuActive(gameOverPanel, true);
        SaveFinalStats();
        PlayerPrefs.DeleteKey("HasValidSave");
    }


    private void SaveFinalStats()
    {
        int survivalDays = TimeManager.Instance.elapsedDays;
        survivalDaysText.text = "Survival Days: " + survivalDays.ToString() + " days";
        if(!PlayerPrefs.HasKey("DaysRecord"))
        {
            PlayerPrefs.SetInt("DaysRecord", survivalDays);
        }
        else if (survivalDays > PlayerPrefs.GetInt("DaysRecord", 0))
        {
            PlayerPrefs.SetInt("DaysRecord", survivalDays);
        }
        PlayerPrefs.Save();
    }


    #endregion


    #region 工具方法
    private void InitialSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleSceneChange(Scene current, Scene next)
    {
        if (next.name == mainMenuScene)
        {
            CurrentState = GameState.PreGame;
            StopAutoSave();
        }
    }

    private bool OnWantsToQuit()
    {
        if (!_isQuitting)
        {
            SaveGame();
        }
        return true;
    }
    #endregion



    private void ShowPauseMenu()
    {
        if (CurrentState != GameState.Paused) return;
        
        StartCoroutine(TransitionMenu(pauseMenuPanel));
    }


    private IEnumerator TransitionMenu(GameObject targetMenu, GameObject previousMenu = null)
    {
        if (_isTransitioning || targetMenu == null) yield break;
        
        _isTransitioning = true;

        // 隐藏当前顶层菜单
        if (_activeMenuStack.Count > 0)
        {
            GameObject currentTop = _activeMenuStack.Peek();
          //  yield return StartCoroutine(FadeMenu(currentTop, 1, 0));
            SetMenuActive(currentTop, false);
        }

        // 显示新菜单
        SetMenuActive(targetMenu, true);
        _activeMenuStack.Push(targetMenu);
     //   yield return StartCoroutine(FadeMenu(targetMenu, 0, 1));

        // 处理返回栈
        if (previousMenu != null && _activeMenuStack.Contains(previousMenu))
        {
            _activeMenuStack.Push(previousMenu);
        }

        _isTransitioning = false;
       // SelectDefaultButton();
       yield return null;
    }

    private IEnumerator FadeMenu(GameObject menu, float startAlpha, float endAlpha)
    {
        CanvasGroup group = menu.GetComponent<CanvasGroup>();
        if (group == null) group = menu.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        while (elapsed < menuFadeDuration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / menuFadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        group.alpha = endAlpha;
    }

    public void CloseTopMenu()
    {
        if (_activeMenuStack.Count == 0 || _isTransitioning) return;

        StartCoroutine(CloseMenuRoutine());
    }

    private IEnumerator CloseMenuRoutine()
    {
        _isTransitioning = true;
        GameObject topMenu = _activeMenuStack.Pop();
       // yield return StartCoroutine(FadeMenu(topMenu, 1, 0));
        SetMenuActive(topMenu, false);

        // 显示下层菜单
        if (_activeMenuStack.Count > 0)
        {
            GameObject nextMenu = _activeMenuStack.Peek();
            SetMenuActive(nextMenu, true);
          //  yield return StartCoroutine(FadeMenu(nextMenu, 0, 1));
        }

        _isTransitioning = false;
        yield return null;

    }

    private void SetMenuActive(GameObject menu, bool active)
    {
        if (menu != null)
        {
            menu.SetActive(active);
            if (active) menu.transform.SetAsLastSibling();
        }
    }
}