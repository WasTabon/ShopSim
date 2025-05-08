using System.Collections;
using UnityEngine;

public class DayTimeController : MonoBehaviour
{
    [SerializeField] private float _dayTime;
    [SerializeField] private Light _directionalLight;

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;
    
    public Material daySkybox;
    public Material nightSkybox;

    private bool isNight;

    private void Start()
    {
        StartCoroutine(SwitchToNightAfterDelay());
    }

    private IEnumerator SwitchToNightAfterDelay()
    {
        yield return new WaitForSeconds(_dayTime);
        SetNight();
    }

    private void SetNight()
    {
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();
        _directionalLight.color = _nightColor;
        isNight = true;
    }

    private void SetDay()
    {
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
        _directionalLight.color = _dayColor;
        isNight = false;
    }

    public void SwitchToDay()
    {
        if (isNight)
        {
            SetDay();
            StartCoroutine(SwitchToNightAfterDelay());
        }
    }
}
