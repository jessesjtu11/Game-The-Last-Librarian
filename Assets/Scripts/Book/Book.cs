using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Book : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string bookName;
    [SerializeField] private string bookDescription;
    [SerializeField] private SkillType skill;
    [SerializeField] private float burnHeatValue = 10f;
    [SerializeField] private float readTimeCost = 30f;

    [Header("UI")]
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private TextMeshProUGUI description;
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

    public void Initialize(string name, string desc, SkillType skillType)
    {
        bookName = name;
        bookDescription = desc;
        skill = skillType;
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
        // pause the game
        // GameManager.Instance.PauseGame();
        
        // display the info of the book
        Debug.Log($"Displaying book: {bookName}");
        infoCanvas.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        burnButton.gameObject.SetActive(true);
        readButton.gameObject.SetActive(true);
    }



    public void OnBurnSelected()
    {
        if (!isInteractable) return;

        // update Heat & Time
        //SurvivalSystem.Instance.AddHeat(burnHeatValue);
       // TimeManager.Instance.AddTime(10f); 
        isInteractable = false;
        isBurned = true;
        HasDecision = true;
        IsAvailable = false;
        
        ClosePanel();
        Debug.Log("Burning the book...");
    }

    public void OnReadSelected()
    {
        if (!isInteractable) return;

      //  TimeManager.Instance.SpendTime(readTimeCost);
      //  SkillSystem.Instance.AcquireSkill(skill);
        isInteractable = false;
        isRead = true;
        HasDecision = true;
        IsAvailable = false;

        ClosePanel();
        Debug.Log("Reading the book...");
    }


    private void ClosePanel()
    {
        infoCanvas.gameObject.SetActive(false);
        //GameManager.Instance.ResumeGame();
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
                               + $"阅读需要时间: {readTimeCost}秒\n"
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