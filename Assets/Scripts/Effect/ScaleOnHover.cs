using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 鼠标悬停时放大并播放音效，移开时缩小并播放音效
/// 需要 DOTween 插件支持
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AudioSource))]
public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("放大比例")]
    [SerializeField] private float hoverScale = 1.2f;

    [Header("动画时间")]
    [SerializeField] private float duration = 0.2f;

    [Header("音效设置")]
    [SerializeField] private AudioClip enterSound;   // 鼠标进入时的音效
    [SerializeField] private float soundVolume = 0.8f; // 音效音量

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Tween currentTween;
    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        // 确保 AudioSource 存在
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        enterSound = Resources.Load<AudioClip>("Sound/Move");
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {

        PlayScaleAnimation(hoverScale);
        PlaySound(enterSound);
    }

    /// <summary>
    /// 鼠标离开
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        PlayScaleAnimation(1f);
    }

    /// <summary>
    /// 播放缩放动画
    /// </summary>
    private void PlayScaleAnimation(float targetScale)
    {
        currentTween?.Kill();
        currentTween = rectTransform.DOScale(originalScale * targetScale, duration)
                                    .SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }
}
