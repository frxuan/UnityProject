using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeToBlack : Singleton<FadeToBlack>
{
    // 拖入一个全屏黑色Image
    public Image fadeImage;

    // 黑屏总耗时（秒）
    public float fadeDuration = 2f;

    void Start()
    {
        // 初始化：完全透明
        //SetAlpha(0);
    }

    /// <summary>
    /// 缓慢黑屏（外部调用）
    /// </summary>
    public void StartFadeToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }

    /// <summary>
    /// 缓慢黑屏协程
    /// </summary>
    IEnumerator FadeToBlackCoroutine()
    {
        float elapsedTime = 0f;

        // 从透明 → 黑色平滑过渡
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        // 确保最后完全变黑
        SetAlpha(1f);
    }

    /// <summary>
    /// 设置黑屏图片透明度
    /// </summary>
    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    // 可选：从黑慢慢变透明
    public void StartFadeToClear()
    {
        StartCoroutine(FadeToClearCoroutine());
    }

    IEnumerator FadeToClearCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
    }
}