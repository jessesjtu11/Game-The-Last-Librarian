using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class BookButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
   // [SerializeField] private Image background;

    public void Initialize(string title, System.Action onClick, Color stateColor)
    {
        titleText.text = title;
     //   background.color = stateColor;
        
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }
}
