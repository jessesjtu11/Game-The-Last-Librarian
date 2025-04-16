using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BookShelf : MonoBehaviour
{
    [Header("书籍配置")]
    [SerializeField] private List<Book> bookList = new List<Book>();
    
    [Header("UI组件")]
    [SerializeField] private GameObject bookListPanel;
    [SerializeField] private GameObject bookButtonPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private float buttonSpacing = 10f;

    private VerticalLayoutGroup layoutGroup;
    private bool isUIOpen = false;
    private bool isProcessBook = false;

    private void Awake()
    {
        InitializeUI();
        UpdateBookDisplay();
        Debug.Log("书架初始化完成");
    }

    private void Start()
    {
        // 这里可以添加一些初始化逻辑，比如加载书籍数据等
        foreach (Book book in bookList)
        {
            book.gameObject.SetActive(false); // 隐藏书籍对象
        }
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isUIOpen)
                CloseBookList();
            else
                gameObject.SetActive(false); 
        }
    }

    
    public void InitializeShelf(List<Book> books)
    {
        bookList = books;
        UpdateBookDisplay();
    }

    private void InitializeUI()
    {
        layoutGroup = contentParent.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = contentParent.gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = buttonSpacing;
        }
        
        bookListPanel.SetActive(false);
    }


    public void OnMouseDown()   //应该要改成点击shelf
    {
        Debug.Log("书架被点击了");
        if (isUIOpen && !isProcessBook)
        {
            // 如果UI已经打开且没有正在处理书籍，则关闭UI
            CloseBookList();
        }
        else if (!isUIOpen)
        {
            DisplayBookList();
        }
    }

    private void DisplayBookList()
    {   isUIOpen = true;
        isProcessBook = true;
        bookListPanel.SetActive(true);
        //GameManager.Instance.PauseGame();
        UpdateBookDisplay();
    }

    private void CloseBookList()
    {
        isUIOpen = false;
        isProcessBook = false;
        bookListPanel.SetActive(false);
        //GameManager.Instance.ResumeGame();
    }

    private void UpdateBookDisplay()
    {
        // 清除旧按钮
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 生成新按钮
        foreach (Book book in bookList)
        {
            CreateBookButton(book);
        }
    }

    private void CreateBookButton(Book book)
    {
        GameObject buttonObj = Instantiate(bookButtonPrefab, contentParent);
        BookButton button = buttonObj.GetComponent<BookButton>();
        
        button.Initialize(
            book.bookName,
            () => OnBookSelected(book),
            book.IsAvailable ? Color.white : Color.gray
        );
    }

    public void OnBookSelected(Book selectedBook)
    {
        Debug.Log($"选择了书籍: {selectedBook.bookName}");
        if (!selectedBook.IsAvailable) return;
        bookListPanel.SetActive(false);
        selectedBook.gameObject.SetActive(true);

        // 触发书籍交互流程
        StartCoroutine(BookSelectionProcess(selectedBook));
    }

    private IEnumerator BookSelectionProcess(Book book)
    {
        Debug.Log($"开始处理书籍: {book.bookName}");
        book.Display();
        
        // 等待玩家决策
        while (!book.HasDecision)
        {
            yield return null;
        }
        bookListPanel.SetActive(true);

        // 处理决策结果
        if (book.isBurned)
        {
            RemoveBook(book);
            Debug.Log($"{book.bookName} 已被烧毁");
        }
        else if (book.isRead)
        {
            RemoveBook(book);
            Debug.Log($"{book.bookName} 已被阅读");
        }

        CloseBookList();
    }

    public void AddBook(Book newBook)
    {
        if (!bookList.Contains(newBook))
        {
            bookList.Add(newBook);
            UpdateBookDisplay();
        }
    }

    public void RemoveBook(Book bookToRemove)
    {
        if (bookList.Contains(bookToRemove))
        {
            bookList.Remove(bookToRemove);
            Destroy(bookToRemove.gameObject); 
            UpdateBookDisplay();
        }
    }


}
