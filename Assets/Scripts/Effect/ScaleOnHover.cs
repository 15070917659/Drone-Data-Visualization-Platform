using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �����ͣʱ�Ŵ󲢲�����Ч���ƿ�ʱ��С��������Ч
/// ��Ҫ DOTween ���֧��
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AudioSource))]
public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("�Ŵ����")]
    [SerializeField] private float hoverScale = 1.2f;

    [Header("����ʱ��")]
    [SerializeField] private float duration = 0.2f;

    [Header("��Ч����")]
    [SerializeField] private AudioClip enterSound;   // ������ʱ����Ч
    [SerializeField] private float soundVolume = 0.8f; // ��Ч����

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Tween currentTween;
    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        // ȷ�� AudioSource ����
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        enterSound = Resources.Load<AudioClip>("Sound/Move");
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {

        PlayScaleAnimation(hoverScale);
        PlaySound(enterSound);
    }

    /// <summary>
    /// ����뿪
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        PlayScaleAnimation(1f);
    }

    /// <summary>
    /// �������Ŷ���
    /// </summary>
    private void PlayScaleAnimation(float targetScale)
    {
        currentTween?.Kill();
        currentTween = rectTransform.DOScale(originalScale * targetScale, duration)
                                    .SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }
}
