using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ReadingRoom : Room
{
    [Header("书籍生成配置")]
    [SerializeField] private BookConfig[] possibleBooks;
    [SerializeField] private int minBooks = 3;
    [SerializeField] private int maxBooks = 8;    
    [SerializeField] private GameObject bookPrefab; 
    [SerializeField] private Transform bookContainer; 
    
    [Header("书架引用")]
    [SerializeField] private BookShelf bookShelf;
    [SerializeField] private GameObject bookShelfPrefab;

    
    private List<Book> currentBooks = new List<Book>();

    protected override void Interact(string objectName)
    {
        GameManager.Instance.Pause_game();
        if (objectName == "BookShelf" && bookShelfPrefab != null)
        {
            bookShelfPrefab.SetActive(true);
        }
    }


    protected override void Start()
    {
        base.Start();
        GenerateBookCollection();
    }

    public override void Load_room_scene()
    {
        base.Load_room_scene();
        RefreshBookShelf();
    }

    private void GenerateBookCollection()
    {
        currentBooks.Clear();
    
        List<BookConfig> remainingConfigs = new List<BookConfig>(possibleBooks);

        float totalProbability = remainingConfigs.Sum(b => b.spawnProbability);
    
        // 计算实际可生成的数量（不超过配置数量）
        int maxPossibleBooks = Mathf.Min(maxBooks, remainingConfigs.Count);
        int bookCount = Random.Range(
            Mathf.Min(minBooks, maxPossibleBooks), 
            maxPossibleBooks + 1
        );

        for (int i = 0; i < bookCount; i++)
        {
            if (remainingConfigs.Count == 0 || totalProbability <= 0) break;

            float randomPoint = Random.Range(0, totalProbability);
            float probabilitySum = 0;
            BookConfig selectedConfig = null;

            // 遍历剩余配置寻找匹配项
            foreach (BookConfig config in remainingConfigs)
            {
                probabilitySum += config.spawnProbability;
                if (randomPoint <= probabilitySum)
                {
                    selectedConfig = config;
                    break;
                }
            }

            // 容错：选择最后一个配置（处理浮点误差）
            if (selectedConfig == null)
            {
                selectedConfig = remainingConfigs[remainingConfigs.Count - 1];
            }

            // 生成书籍并更新概率
            CreateBookInstance(selectedConfig);
            remainingConfigs.Remove(selectedConfig);
            totalProbability -= selectedConfig.spawnProbability;
        }
    }


    private void CreateBookInstance(BookConfig config)
{
    // 实例化书籍预制体（需要先在编辑器中配置bookPrefab）
    GameObject bookObj = Instantiate(bookPrefab);
    bookObj.transform.SetParent(bookContainer); // 设置父物体保持场景整洁
    
    // 获取Book组件并初始化
    Book newBook = bookObj.GetComponent<Book>();
    if (newBook != null)
    {
        newBook.Initialize(
            config.bookName,
            config.description,
            config.associatedSkill
        );
        
        currentBooks.Add(newBook);
    }
    else
    {
        Debug.LogError("实例化的书籍预制体缺少Book组件");
        Destroy(bookObj);
    }
}


    private void RefreshBookShelf()
    {
        if (bookShelf != null)
        {
            bookShelf.InitializeShelf(currentBooks);
        }
    }



}

