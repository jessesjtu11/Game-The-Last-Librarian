// TimeManager.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("时间设置")]
    [SerializeField, Tooltip("现实时间秒数对应游戏1天")]  private float realSecondsPerDay = 120f;
    [SerializeField, Tooltip("时间流逝速度倍率")]  public float timeScale = 1f;

    [Header("UI组件")]
    [SerializeField] private RectTransform clockPointer;
    [SerializeField] private TextMeshProUGUI daysCounterText;

    public int elapsedDays;
    private float rotationProgress; // 0-1表示当日进度\


    private void Awake()
    {
        InitializeSingleton();
        LoadTimeData();
    }

    private void InitializeSingleton()
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

    private void Update()
    {
       
            UpdateTimeProgress();
            UpdateClockVisual();
        
    }

    private void UpdateTimeProgress()
    {
        rotationProgress += Time.deltaTime * timeScale / realSecondsPerDay;
        
        if (rotationProgress >= 1f)
        {
            StartCoroutine(NewDayRoutine());
        }
    }

    public void SetTimeScale(float newScale)
    {
        timeScale = newScale;
    }

    public void AddTime(float amount)
    {
        rotationProgress += amount / realSecondsPerDay;
        if (rotationProgress >= 1f)
        {
            StartCoroutine(NewDayRoutine());
        }
    }

    private IEnumerator NewDayRoutine()
    {
        elapsedDays++;
        rotationProgress = 0f;
        UpdateDaysCounter();
        
        // 触发新的一天事件
        yield return StartCoroutine(OnNewDayStart());
        
        SaveTimeData();
    }

    private void UpdateClockVisual()
    {
        float targetRotation = 360f * rotationProgress;
        clockPointer.rotation = Quaternion.Euler(0, 0, -targetRotation);
    }

    private void UpdateDaysCounter()
    {
        daysCounterText.text = $"Yon've survived for {elapsedDays} days";
        // 添加文本缩放动画
        LeanTween.scale(daysCounterText.gameObject, Vector3.one * 1.2f, 0.3f)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1);
    }


    private IEnumerator OnNewDayStart()
    {
        
        // 示例：渐隐过渡效果
        yield return StartCoroutine(ScreenFader.Instance.FadeOut(1f));
        // 这里可以添加其他新天数逻辑
        yield return StartCoroutine(ScreenFader.Instance.FadeIn(1f));

        
    } 

    #region 数据持久化
    public void SaveTimeData()
    {
        PlayerPrefs.SetInt("ElapsedDays", elapsedDays);
        PlayerPrefs.SetFloat("DayProgress", rotationProgress);
    }

    public void LoadTimeData()
    {
        elapsedDays = PlayerPrefs.GetInt("ElapsedDays", 0);
        rotationProgress = PlayerPrefs.GetFloat("DayProgress", 0f);
        UpdateDaysCounter();
    }

    public void ResetToDayZero()
    {
        elapsedDays = 0;
        rotationProgress = 0f;
        UpdateDaysCounter();
        SaveTimeData();
    }
    #endregion

}
