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
    
    [Header("书架引用")]
    [SerializeField] private BookShelf bookShelf;
    [SerializeField] private GameObject bookShelfPrefab;
    
    private List<Book> currentBooks = new List<Book>();

    protected override void Interact(string objectName)
    {
        if (objectName == "BookShelf" && bookShelfPrefab != null)
        {
            bookShelfPrefab.SetActive(true);
        }
    }

/*
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
        
        // 计算总概率
        float totalProbability = possibleBooks.Sum(b => b.spawnProbability);
        
        // 生成随机数量
        int bookCount = Random.Range(minBooks, maxBooks + 1);
        
        for (int i = 0; i < bookCount; i++)
        {
            float randomPoint = Random.Range(0, totalProbability);
            float probabilitySum = 0;

            foreach (BookConfig config in possibleBooks)
            {
                probabilitySum += config.spawnProbability;
                if (randomPoint <= probabilitySum)
                {
                    CreateBookInstance(config);
                    break;
                }
            }
        }
    }

    private void CreateBookInstance(BookConfig config)
    {
        Book newBook = ScriptableObject.CreateInstance<Book>();
        newBook.Initialize(
            config.bookName,
            config.description,
            config.associatedSkill
        );
        
        currentBooks.Add(newBook);
    }

    private void RefreshBookShelf()
    {
        if (bookShelf != null)
        {
            bookShelf.InitializeShelf(currentBooks);
        }
    }



    // 编辑器工具：验证概率设置
    private void OnValidate()
    {
        if (possibleBooks != null)
        {
            float totalProb = possibleBooks.Sum(b => b.spawnProbability);
            if (totalProb < 0.1f)
            {
                Debug.LogWarning("总生成概率过低，可能无法生成书籍！");
            }
        }
    }*/
}

