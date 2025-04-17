using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class PlayerStatusUI : MonoBehaviour
{
    [Header("体温显示")]
    [SerializeField] private Slider temperatureBar;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private Color normalColor = new Color(0.2f, 0.6f, 1f, 0.8f);
    [SerializeField] private Color frozenColor = new Color(1f, 0.3f, 0.2f, 0.8f);
    [SerializeField] private float colorChangeThreshold = 35f;

    private void Start()
    {
        // 初始化状态栏
        temperatureBar.minValue = Player.Instance.minBodyTemp;
        temperatureBar.maxValue = Player.Instance.maxBodyTemp;
        
        // 注册体温更新事件
       // Player.Instance.OnTemperatureChanged += UpdateTemperatureDisplay;
        
        // 初始更新
        UpdateTemperatureDisplay();
    }

    private void Update()
    {
            UpdateTemperatureDisplay();
    }

    private void UpdateTemperatureDisplay()
    {
        float currentTemp = Player.Instance.currentBodyTemp;
        
        // 平滑过渡动画
        LeanTween.value(gameObject, temperatureBar.value, currentTemp, 0.5f)
            .setOnUpdate(SetBarValue)
            .setEase(LeanTweenType.easeOutQuad);

        // 颜色变化逻辑
        fillImage.color = (currentTemp < colorChangeThreshold ) ? 
            Color.Lerp(normalColor, frozenColor, (colorChangeThreshold - currentTemp) / 2f) : 
            normalColor;

        // 温度文本
        temperatureText.text = $"{currentTemp:00.0}";
    }

    private void SetBarValue(float value)
    {
        temperatureBar.value = value;
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnTemperatureChanged -= UpdateTemperatureDisplay;
        }
    }

}
