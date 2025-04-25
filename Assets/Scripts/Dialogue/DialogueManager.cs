using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    [Header("UI Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text dialogueText;
    [SerializeField] private CanvasGroup dialogueBox;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Input")]
    [SerializeField] private KeyCode advanceKey = KeyCode.Mouse0;


    [SerializeField] private DialogueData dialogueData;
    private Queue<DialogueData.DialogueSegment> dialogueQueue = new Queue<DialogueData.DialogueSegment>();
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private AudioSource audioSource;

    private void Awake()
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
        audioSource = GetComponent<AudioSource>();
        dialogueBox.alpha = 0;
        backgroundImage.color = Color.clear;
    }

    private void Start()
    {
        // 开始对话
        StartDialogue(dialogueData);
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        dialogueQueue.Clear();
        foreach (var segment in dialogueData.dialogueSegments)
        {
            dialogueQueue.Enqueue(segment);
        }

        StartCoroutine(StartDialogueRoutine());
    }

    private IEnumerator StartDialogueRoutine()
    {
        // 初始淡入
        yield return FadeCanvasGroup(dialogueBox, 0, 1, fadeDuration);

        while (dialogueQueue.Count > 0)
        {
            var segment = dialogueQueue.Dequeue();

            // 背景切换
            if (segment.backgroundImage != null)
            {
                yield return SwitchBackground(segment.backgroundImage);
            }

            // 开始显示文字
            typingCoroutine = StartCoroutine(TypeText(segment));
            
            // 等待输入
            yield return WaitForAdvance(typingCoroutine, segment.dialogueText);
            
            // 清除文字
            dialogueText.text = "";
        }

        // 结束淡出
        yield return FadeCanvasGroup(dialogueBox, 1, 0, fadeDuration);
        
        // 后续事件触发（比如场景切换）...
        Debug.Log("剧情动画结束");
        StartCoroutine(GameManager.Instance.LoadGameScene("Game",false));
    }

    private IEnumerator TypeText(DialogueData.DialogueSegment segment)
    {
        isTyping = true;
        dialogueText.text = "";
        string originalText = segment.dialogueText;
        int currentChar = 0;
        //bool isRichText = false;

        while (currentChar < originalText.Length)
        {
            /* 处理富文本标签
            if (originalText[currentChar] == '<')
            {
                int closeIndex = originalText.IndexOf('>', currentChar);
                if (closeIndex != -1)
                {
                    dialogueText.text += originalText.Substring(currentChar, closeIndex - currentChar + 1);
                    currentChar = closeIndex + 1;
                    isRichText = !isRichText;
                    continue;
                }
            }*/

            dialogueText.text += originalText[currentChar];
            currentChar++;             
            
            yield return new WaitForSeconds(segment.textSpeed);
        }

        isTyping = false;
    }

    private IEnumerator WaitForAdvance(Coroutine typingRoutine, string currentText)
    {// 等待用户输入进入下一段对话
        while (true)
        {
            if (Input.GetKeyDown(advanceKey))
            {
                if (isTyping)
                {
                    // 快速完成当前打字
                    StopCoroutine(typingRoutine);
                    dialogueText.text = currentText;
                    isTyping = false;
                }
                else
                {
                    // 进入下一段对话
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator SwitchBackground(Sprite newBackground)
    {
        // 淡出当前背景
        yield return FadeImage(backgroundImage, 1, 0, fadeDuration/2);
        
        // 切换背景
        backgroundImage.sprite = newBackground;
        backgroundImage.color = Color.white;
        
        // 淡入新背景
        yield return FadeImage(backgroundImage, 0, 1, fadeDuration/2);
    }

    #region 动画效果
    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to,float duration )
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        group.alpha = to;
    }

    private IEnumerator FadeImage(Image image, float from, float to, float duration)
    {
        float elapsed = 0;
        Color color = image.color;
        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(from, to, elapsed / duration);
            image.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        color.a = to;
        image.color = color;
    }
    #endregion
}