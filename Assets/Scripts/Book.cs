using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Book : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string bookName;
    [SerializeField] private SkillType skill;
    [SerializeField] private float burnHeatValue = 10f;
    [SerializeField] private float readTimeCost = 30f;

    [Header("UI")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button burnButton;
    [SerializeField] private Button readButton;

    private bool isInteractable = true;

    private void Start()
    {
        
        infoPanel.SetActive(false);
        description.text = bookName;
        
    }

    private void OnMouseDown()
    {
        if (isInteractable)
        {
            Display();
        }
    }

    private void Display()
    {
        // pause the game
        // GameManager.Instance.PauseGame();
        
        // display the info of the book
        infoPanel.SetActive(true);
        description.gameObject.SetActive(true);
        burnButton.gameObject.SetActive(true);
        readButton.gameObject.SetActive(true);
    }



    public void OnBurnSelected()
    {
        Burn_book();
        ClosePanel();
        Debug.Log("Burning the book...");
    }

    public void OnReadSelected()
    {
        Read_book();
        ClosePanel();
        Debug.Log("Reading the book...");
    }

    public void Burn_book()
    {
        if (!isInteractable) return;

        // update Heat & Time
        SurvivalSystem.Instance.AddHeat(burnHeatValue);
        TimeManager.Instance.AddTime(10f); 
        
        DestroyBook();
    }

    public void Read_book()
    {
        if (!isInteractable) return;

        TimeManager.Instance.SpendTime(readTimeCost);
        SkillSystem.Instance.AcquireSkill(skill);
        
        DestroyBook();  // Destroy the book after reading
    }

    private void ClosePanel()
    {
        infoPanel.SetActive(false);
        //GameManager.Instance.ResumeGame();
    }

    private void DestroyBook()
    {
        isInteractable = false;
        //BookSpawner.Instance.RemoveBookFromCurrentRoom(this);
        Destroy(gameObject);
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