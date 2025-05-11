using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Loading : MonoBehaviour
{
    [Header("UI Elements")]
    public Image logoImage;

    [Header("Audio")]
    public AudioClip popSound;
    public AudioClip shineSound;

    [Header("Next Scene")]
    public string nextSceneName;

    public Ease _ease;

    private Vector3 originalScale;
    private Material logoMaterial;

    private AudioSource _audioSource;

    void Awake()
    {
        // Сохраняем исходный размер и скрываем логотип
        originalScale = logoImage.rectTransform.localScale;
        logoImage.rectTransform.localScale = Vector3.zero;

        _audioSource = GetComponent<AudioSource>();
        
        // Получаем материал (дублируем, чтобы не менять оригинал)
        logoMaterial = logoImage.material;
        logoImage.material = new Material(logoMaterial);
        logoMaterial = logoImage.material;
        logoMaterial.SetFloat("_ShineLocation", 0f);
    }

    void Start()
    {
        // Анимация "выпрыгивания"
        logoImage.rectTransform
            .DOScale(originalScale, 2.749f)
            .SetEase(_ease)
            .OnStart(() =>
            {
                _audioSource.PlayOneShot(popSound);
            })
            .OnComplete(() =>
            {
                _audioSource.PlayOneShot(shineSound);
                DOTween.To(
                    () => logoMaterial.GetFloat("_ShineLocation"),
                    x => logoMaterial.SetFloat("_ShineLocation", x),
                    1f, // конечное значение
                    2.996f // длительность
                ).OnComplete(() =>
                {
                    // Загрузка следующей сцены
                    SceneManager.LoadScene(nextSceneName);
                });
            });
    }
}