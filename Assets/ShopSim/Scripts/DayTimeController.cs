using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    public static DayTimeController Instance { get; private set; }

    [SerializeField] private Image _dayTimeTransitionImage;

    [SerializeField] private float _dayTime;
    [SerializeField] private float _transitionTime;
    [SerializeField] private Light _directionalLight;

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;

    public Material daySkybox;
    public Material nightSkybox;

    public bool IsDay;
    public bool IsNight;

    private bool _isNight;

    private Quaternion _baseRotation;
    private Color _baseColor;

    private Quaternion _nightRotation = Quaternion.Euler(318.345337f, 41.8368301f, 163.594666f);
    private float _timer;

    private void Awake()
    {
        Instance = this;
        IsDay = true;
        _dayTimeTransitionImage.gameObject.SetActive(true);
        _dayTimeTransitionImage.DOFade(0f, 0f);
        _dayTimeTransitionImage.gameObject.SetActive(false);

        _baseRotation = _directionalLight.transform.rotation;
        _baseColor = _directionalLight.color;
    }

    private void Start()
    {
        StartCoroutine(SwitchToNightAfterDelay());
    }

    private IEnumerator SwitchToNightAfterDelay()
    {
        _timer = 0f;
        while (_timer < _dayTime)
        {
            _timer += Time.deltaTime;
            float progress = Mathf.Clamp01(_timer / _dayTime);

            _directionalLight.transform.rotation = Quaternion.Lerp(_baseRotation, _nightRotation, progress);
            _directionalLight.color = Color.Lerp(_baseColor, _nightColor, progress);

            yield return null;
        }
        SetNight();
    }

    private void SetNight()
    {
        _dayTimeTransitionImage.DOKill();
        _dayTimeTransitionImage.gameObject.SetActive(true);
        _dayTimeTransitionImage.DOFade(1, _transitionTime)
            .OnComplete(() =>
        {
            ChangeSkyboxNight();
            _dayTimeTransitionImage.DOFade(0, _transitionTime)
                .OnComplete(() =>
                {
                    IsNight = true;
                    IsDay = false;
                    _isNight = true;
                    _dayTimeTransitionImage.gameObject.SetActive(false);
                });
        });
    }

    private void ChangeSkyboxNight()
    {
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();
        _directionalLight.color = _nightColor;
        _directionalLight.transform.rotation = _nightRotation;
    }

    private void SetDay()
    {
        _dayTimeTransitionImage.DOKill();
        _dayTimeTransitionImage.gameObject.SetActive(true);
        _dayTimeTransitionImage.DOFade(1, _transitionTime)
            .OnComplete(() =>
            {
                SetSkyboxDay();
                _dayTimeTransitionImage.DOFade(0, _transitionTime)
                    .OnComplete(() =>
                    {
                        IsDay = true;
                        IsNight = false;
                        _isNight = false;
                        _dayTimeTransitionImage.gameObject.SetActive(false);
                        ResetLight();
                    });
            });
    }

    private void SetSkyboxDay()
    {
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
        _directionalLight.color = _dayColor;
    }

    private void ResetLight()
    {
        _directionalLight.transform.rotation = _baseRotation;
        _directionalLight.color = _baseColor;
    }

    public void SwitchToDay()
    {
        if (_isNight)
        {
            SetDay();
            StartCoroutine(SwitchToNightAfterDelay());
        }
    }
}
