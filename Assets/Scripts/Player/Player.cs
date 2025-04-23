using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    #region 状态属性
    [Header("体温设置")]
    [SerializeField, Tooltip("基础体温值")] private float baseBodyTemp = 36.5f;
    [SerializeField, Tooltip("最大允许体温")]  public float maxBodyTemp = 39f;
    [SerializeField, Tooltip("最低允许体温")]  public float minBodyTemp = 30f;
    [SerializeField] public float currentBodyTemp;
    
   // private List<Book> bookBag = new List<Book>();
//    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType>();

    public int currentRoomIndex=1; // 当前房间索引
    #endregion

    #region 事件系统
    public delegate void PlayerStateUpdate();
    public event PlayerStateUpdate OnTemperatureChanged;
    public event PlayerStateUpdate OnInventoryUpdated;
   // public event PlayerStateUpdate OnSkillUnlocked;
    #endregion

    private void Awake()
    {
        InitializeSingleton();
        LoadPlayerState();  //还没看
        InitializeTemperatureSystem();
    }

    #region 初始化
    private void InitializeSingleton() //单例初始化
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

    private void InitializeTemperatureSystem()
    {
        currentBodyTemp = baseBodyTemp;
    }
    #endregion


    #region 体温管理
    public void ModifyTemperature(float amount)
    {
        currentBodyTemp += amount;
        CheckTemperatureEffects(); // 检查体温状态效果
    }

    private void CheckTemperatureEffects()
    {
        // 根据体温触发不同状态效果
        if (currentBodyTemp >= 39f)
        {
            Debug.LogWarning("进入高热状态！");
        }
        else if (currentBodyTemp <= 35f)
        {
            Debug.LogWarning("进入低温症状态！");
        }
    }
    #endregion

    /*
    public bool RemoveBook(Book targetBook)
    {
        bool result = bookBag.Remove(targetBook);
        if (result) OnInventoryUpdated?.Invoke();
        return result;
    } 
    
    public void AddBook(Book newBook)
    {
        if (!bookBag.Contains(newBook))
        {
            bookBag.Add(newBook);
            OnInventoryUpdated?.Invoke();
            
             //改为调用SkillSystem
            if (newBook.associatedSkill != SkillType.None)
            {
                SkillSystem.Instance.UnlockSkill(newBook.associatedSkill);
            }
        }
    } */
    


    // 数据持久化部分修改
    public void SavePlayerState()
    {
        // 移除了技能保存
        PlayerPrefs.SetFloat("BodyTemperature", currentBodyTemp);
        PlayerPrefs.SetInt("CurrentRoom", currentRoomIndex);
        
       // string bookNames = string.Join(",", bookBag.ConvertAll(b => b.bookName));
       // PlayerPrefs.SetString("BookBag", bookNames);
    }

    public void LoadPlayerState()
    {
        currentBodyTemp = PlayerPrefs.GetFloat("BodyTemperature", baseBodyTemp);
        currentRoomIndex = PlayerPrefs.GetInt("CurrentRoom", 1);

        /*string[] bookNames = PlayerPrefs.GetString("BookBag").Split(',');
        foreach(string name in bookNames)
        {
            Book book = BookDatabase.Instance.GetBookByName(name);
            if (book != null) AddBook(book);
            
        } */
    }

    public void ResetPlayerState()
    {
        currentBodyTemp = baseBodyTemp;
        currentRoomIndex = 1; // 重置为起始房间
        SavePlayerState();
    }
}

#region 辅助枚举

public enum RoomType
{
    Lounge,
    Library,
    Laboratory,
    Archive,
    Greenhouse
}
#endregion