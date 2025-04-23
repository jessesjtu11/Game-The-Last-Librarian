using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1f;

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
    }

    public IEnumerator FadeOut(float duration)
    {
        float alpha = 0f;
        fadeImage.color = new Color(0, 0, 0, alpha);
        fadeImage.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += Time.deltaTime / duration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        float alpha = 1f;
        fadeImage.color = new Color(0, 0, 0, alpha);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / duration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }
}