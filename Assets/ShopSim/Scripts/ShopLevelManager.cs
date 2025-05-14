using ShopSim.Scripts;
using TMPro;
using UnityEngine;

public class ShopLevelManager : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private int[] _levelUpgradePrices;
    [SerializeField] private Transform[] _levelUpgradePositions;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _upgradeButonText;
    [SerializeField] private BuyController _buyController;
    [SerializeField] private GameObject _upgradeParticle;
    [SerializeField] private AudioClip _upgradeSound;

    [SerializeField] private GameObject _level1;
    [SerializeField] private GameObject _level2;
    [SerializeField] private GameObject _level3;
    [SerializeField] private GameObject _level4;
    [SerializeField] private GameObject _level5;
    [SerializeField] private GameObject _level6;
    [SerializeField] private GameObject _level7;
    [SerializeField] private GameObject _level8;
    [SerializeField] private GameObject _level9;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _levelText.text = $"Level {_level}";
        _upgradeButonText.text = $"Upgrade - {_levelUpgradePrices[_level]} <sprite name=\"money 2 1\">";
    }

    public int GetLevel()
    {
        return _level;
    }
    
    public void UpgradeLevel()
    {
        if (_buyController.GetMoneyCount() - _levelUpgradePrices[_level] >= 0)
        {
            _buyController.RemoveMoney(_levelUpgradePrices[_level]);
            ChangeShop();
            Instantiate(_upgradeParticle, _levelUpgradePositions[_level].position, Quaternion.identity);
            _audioSource.PlayOneShot(_upgradeSound);
            _level++;
        }
        else
        {
            _buyController.ShowPanelDontHaveMoney();
        }
    }

    private void ChangeShop()
    {
        switch (_level)
        {
            case 1:
                _level1.SetActive(false);
                break;
            case 2: 
                _level2.SetActive(false);
                break;
            case 3: 
                _level3.SetActive(false);
                break;
            case 4: 
                _level4.SetActive(true);
                break;
            case 5: 
                _level5.SetActive(true);
                break;
            case 6: 
                _level6.SetActive(true);
                break;
            case 7: 
                _level7.SetActive(true);
                break;
            case 8: 
                _level8.SetActive(true);
                break;
            case 9: 
                _level9.SetActive(true);
                _upgradeButton.SetActive(false);
                break;
            case 10:
                break;
            default:
                break;
        }
    }
}
