using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Book : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int bookID;
    [SerializeField] public string bookName;
    [SerializeField] public string bookDescription;
    [SerializeField] private float burnHeatValue = 0.1f;
    [SerializeField] private float readHeatCost = 0.3f;
    [SerializeField] private float readTimeCost = 10f;
    [SerializeField] private float burnTimeCost = 20f;

    [Header("UI")]
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private Text description;
    [SerializeField] private Button burnButton;
    [SerializeField] private Button readButton;

    public bool isInteractable = true;
    public bool isBurned = false;
    public bool isRead = false;
    public bool HasDecision = false;
    public bool IsAvailable = true;

    private void Start()
    {
        
        infoCanvas.gameObject.SetActive(false);
        description.text = bookDescription;
        
    }

    public void Initialize(BookConfig config)
    {
        bookID = config.bookID;
        bookName = config.bookName;
        bookDescription = string.Format("{0} \n热值：{1}  看完所需热量：{2}", config.description, config.burnHeatValue, config.readHeatCost);
        burnHeatValue = config.burnHeatValue;
        readHeatCost = config.readHeatCost;
        readTimeCost = config.readTimeCost;
        burnTimeCost = config.burnTimeCost;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && infoCanvas.gameObject.activeSelf)
        {
            ClosePanel();
        }
    }   

    private void OnMouseDown()
    {
        if (isInteractable)
        {
            Display();
        }
    }


    public void Display()
    {        
        Debug.Log($"Displaying book: {bookName}");
        infoCanvas.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        burnButton.gameObject.SetActive(true);
        readButton.gameObject.SetActive(true);
    }



    public void OnBurnSelected()
    {
        if (!isInteractable) return;

        isInteractable = false;
        isBurned = true;
        HasDecision = true;
        IsAvailable = false;

        Player.Instance.ModifyTemperature(burnHeatValue); 
        TimeManager.Instance.AddTime(burnTimeCost); // 10秒的时间流逝
        
        ClosePanel();
        Debug.Log("Burning the book...");
    }

    public void OnReadSelected()
    {
        if (!isInteractable) return;
        isInteractable = false;
        isRead = true;
        HasDecision = true;
        IsAvailable = false;

        Player.Instance.ModifyTemperature(-readHeatCost); // 体温下降
        TimeManager.Instance.AddTime(readTimeCost); // 10秒的时间流逝
        SkillSystem.Instance.AcquireSkillFromBook(bookID); // 学习技能

        ClosePanel();
        Debug.Log("Reading the book...");
    }


    private void ClosePanel()
    {
        infoCanvas.gameObject.SetActive(false);
    }


}



public enum SkillType
{
    FireResistance,
    SpeedBoost,
    ThermalVision
}

/*
    private void UpdateSkillDisplay()
    {
        // 显示当前技能效果
        skillDescription.text = $"可获得技能: {GetSkillDescription(skill)}\n"
                               + $"阅读需要时间: {readHeatCost}秒\n"
                               + $"燃烧可获得热量: {burnHeatValue}单位";
    }

    private string GetSkillDescription(SkillType skillType)
    {
        // 这里可以扩展为从配置表读取
        return skillType switch
        {
            SkillType.FireResistance => "火焰抗性：提高耐寒能力",
            SkillType.SpeedBoost => "疾速：移动速度提升",
            SkillType.ThermalVision => "热视觉：黑暗中发现热源",
            _ => "未知技能"
        };
    }

*/