using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _postProccesing;
    [SerializeField] private TextMeshProUGUI _graphicsQualityText;
    private bool _isHighQuality = true;

    public void SetGraphicsQuality()
    {
        if (_isHighQuality)
        {
            _graphicsQualityText.text = "Low";
            _postProccesing.SetActive(false);
            _isHighQuality = false;
        }
        else 
        {
            _graphicsQualityText.text = "High";
            _postProccesing.SetActive(true);
            _isHighQuality = true;
        }
    }
}
