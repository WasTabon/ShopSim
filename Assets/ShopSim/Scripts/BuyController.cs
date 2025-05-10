using System;
using System.Collections.Generic;
using DG.Tweening;
using ShopSim.Scripts.Sellers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSim.Scripts
{
    public class BuyController : MonoBehaviour
    {
        [SerializeField] private int _moneyCount;
        [SerializeField] private float _uiPanelFadeTime;
        [SerializeField] private float _uiButtonScaleTimeIn;
        [SerializeField] private float _uiButtonScaleTimeOut;

        [SerializeField] private SellersManager _sellersManager;
        [SerializeField] private ShopLevelManager _shopLevelManager;
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        [SerializeField] private Image _checkPanel;
        [SerializeField] private RectTransform _lootBoxPanel;
        [SerializeField] private RectTransform _settingsPanel;
        [SerializeField] private RectTransform _sellFakeItemPanel;
        [SerializeField] private RectTransform _dontHaveMoneyPanel;
        [SerializeField] private RectTransform _fakePanel;
        [SerializeField] private RectTransform _notFakePanel;
        [SerializeField] private Image _itemsListPanel;
        [SerializeField] private RectTransform _uiButtonCheck;

        [SerializeField] private Sprite _common;
        [SerializeField] private Sprite _rare;
        [SerializeField] private Sprite _epic;
        [SerializeField] private Sprite _legendary;
        
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Image _itemRarity;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemPrice;

        private List<Image> _itemSlots;
        [SerializeField] private Image _itemSlot;
        [SerializeField] private RectTransform _itemSlotsContent;
        [SerializeField] private TextMeshProUGUI _earnedMoneyText;
        
        private int _itemsSoldCount;
        private int _earnedMoneyCount;

        private float _sellItemModifier;
        
        private Vector3 _buyScale;
        private Vector3 _checkScale;
        private Vector3 _sellFakeItemScale;
        private Vector3 _dontHaveMoneyScale;
        private Vector3 _fakeScale;
        private Vector3 _notFakeScale;
        private Vector3 _lootBoxScale;
        private Vector3 _settingsScale;
        private Vector3 _denyScale;

        private bool _isNightPanelOpened;
        private bool _buyFake;
        
        private Seller _currentSeller;

        private void Start()
        {
            _itemSlots = new List<Image>();
            _checkScale = _uiButtonCheck.localScale;
            
            _uiButtonCheck.localScale = Vector3.zero;

            _dontHaveMoneyScale = _dontHaveMoneyPanel.localScale;
            _dontHaveMoneyPanel.DOScale(Vector3.zero, 0f);
            _dontHaveMoneyPanel.gameObject.SetActive(false);
            
            _fakeScale = _fakePanel.localScale;
            _fakePanel.DOScale(Vector3.zero, 0f);
            _fakePanel.gameObject.SetActive(false);
            
            _notFakeScale = _notFakePanel.localScale;
            _notFakePanel.DOScale(Vector3.zero, 0f);
            _notFakePanel.gameObject.SetActive(false);

            _sellFakeItemScale = _sellFakeItemPanel.localScale;
            _sellFakeItemPanel.localScale = Vector3.zero;
            _sellFakeItemPanel.gameObject.SetActive(false);
            
            _settingsScale = _settingsPanel.localScale;
            _settingsPanel.localScale = Vector3.zero;
            _settingsPanel.gameObject.SetActive(false);
            
            _lootBoxScale = _lootBoxPanel.localScale;
            _lootBoxPanel.localScale = Vector3.zero;
            _lootBoxPanel.gameObject.SetActive(false);
            
            float tempFadeTime = _uiPanelFadeTime;
            _uiPanelFadeTime = 0;
            
            SetPanelFade(_checkPanel, 0);
            _checkPanel.gameObject.SetActive(false);
            _uiPanelFadeTime = tempFadeTime;
        }

        private void Update()
        {
            _moneyText.text = _moneyCount.ToString();
            if (DayTimeController.Instance.IsNight && _sellersManager.sellersQueue.Count <= 0 && !_isNightPanelOpened)
            {
                OpenNightPanel();
            }

            if (_shopLevelManager.GetLevel() == 1) _sellItemModifier = 1f;
            else if (_shopLevelManager.GetLevel() == 2) _sellItemModifier = 1.15f;
            else if (_shopLevelManager.GetLevel() == 3) _sellItemModifier = 1.3f;
            else if (_shopLevelManager.GetLevel() == 4) _sellItemModifier = 1.5f;
            else if (_shopLevelManager.GetLevel() == 5) _sellItemModifier = 1.8f;
            else if (_shopLevelManager.GetLevel() == 6) _sellItemModifier = 2f;
            else if (_shopLevelManager.GetLevel() == 7) _sellItemModifier = 2.5f;
            else if (_shopLevelManager.GetLevel() == 8) _sellItemModifier = 3f;
            else if (_shopLevelManager.GetLevel() == 9) _sellItemModifier = 3.5f;
        }

        public void SetCurrentSeller(Seller seller)
        {
            _currentSeller = seller;
            SetButtonScale(_uiButtonCheck, _checkScale, _uiButtonScaleTimeIn, Ease.OutElastic);
            SetItemPanel(_currentSeller.GetItem());
        }
        
        public void Buy()
        {
            if (_moneyCount - _currentSeller.GetItem().GetPrice() >= 0)
            {
                if (_currentSeller != null)
                {
                    _moneyCount -= _currentSeller.GetItem().GetPrice();
                    
                    Image slot = Instantiate(_itemSlot, _itemSlotsContent);
                    slot.sprite = _currentSeller.GetItem().GetIcon();
                    _itemSlots.Add(slot);
                    _earnedMoneyCount += (int)(_currentSeller.GetItem().GetPrice() * _sellItemModifier);
                    _earnedMoneyText.text = _earnedMoneyCount.ToString();
                    _itemsSoldCount++;

                    if (_currentSeller.GetItem().GetFake() && _buyFake == false)
                        _buyFake = true;
                    
                    SetPanelFade(_checkPanel, 0);
                    SetButtonScale(_uiButtonCheck, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
                    _checkPanel.gameObject.SetActive(false);
                    _currentSeller.ItemProccesed(true);
                    _currentSeller = null;
                }
            }
            else
            {
                OpenPanelMessage(_dontHaveMoneyPanel, _dontHaveMoneyScale);
            }
        }

        public void ShowPanelDontHaveMoney()
        {
            OpenPanelMessage(_dontHaveMoneyPanel, _dontHaveMoneyScale);
        }

        public void Verify()
        {
            if (_moneyCount - 50 >= 0)
            {
                if (_currentSeller != null)
                {
                    _moneyCount -= 50;
                    if (_currentSeller.GetItem().GetFake())
                    {
                        OpenPanelMessage(_fakePanel, _fakeScale);
                    }
                    else
                    {
                        OpenPanelMessage(_notFakePanel, _notFakeScale);
                    }
                }   
            }
            else
            {
                OpenPanelMessage(_dontHaveMoneyPanel, _dontHaveMoneyScale);
            }
        }

        public void Deny()
        {
            if (_currentSeller != null)
            {
                SetPanelFade(_checkPanel, 0);
                SetButtonScale(_uiButtonCheck, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
                _checkPanel.gameObject.SetActive(false);
                _currentSeller.ItemProccesed(false);
                _currentSeller = null;
            }
        }

        public void Sell()
        {
            if (_buyFake == false)
            {
                _moneyCount += (int)(_earnedMoneyCount * _sellItemModifier);
                _earnedMoneyCount = 0;
                _itemsSoldCount = 0;
                _buyFake = false;
                CloseSellFakePanel();
            }
            else
            {
                _earnedMoneyCount = 50;
                _moneyCount += _earnedMoneyCount;
                _earnedMoneyCount = 0;
                _itemsSoldCount = 0;
                _buyFake = false;
                _sellFakeItemPanel.gameObject.SetActive(true);
                OpenPanelMessage(_sellFakeItemPanel, _sellFakeItemScale);
                
            }
            foreach (Image itemSlot in _itemSlots)
            {
                Destroy(itemSlot.gameObject);
            }
            _itemSlots.Clear();
        }
        
        public int GetMoneyCount()
        {
            return _moneyCount;
        }

        public void RemoveMoney(int count)
        {
            _moneyCount -= count;
        }
        
        public void CloseSellFakePanel()
        {
            _sellFakeItemPanel.DOKill();
            _sellFakeItemPanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _sellFakeItemPanel.gameObject.SetActive(false);
                    SetPanelFade(_itemsListPanel, 0, SetDayAferSold);
                }));
        }

        private void SetDayAferSold()
        {
            DayTimeController.Instance.SwitchToDay();
            _itemsListPanel.gameObject.SetActive(false);
            Invoke("SetNightPanelOpenedFalse", 7f);
        }

        private void SetNightPanelOpenedFalse()
        {
            _isNightPanelOpened = false;
        }

        private void OpenNightPanel()
        {
            _isNightPanelOpened = true;
            _itemsListPanel.gameObject.SetActive(true);
            SetPanelFade(_itemsListPanel, 1);
        }

        public void OpenCheckPanel()
        {
            _checkPanel.gameObject.SetActive(true);
            SetPanelFade(_checkPanel, 1);
        }

        public void SetPanelFade(Image panel, float fade, Action onComplete = null)
        {
            float tempPanelFade = 0;
            panel.DOKill();
            if (fade == 1)
                tempPanelFade = 0.45f;
            else if (fade == 0)
                tempPanelFade = 0;
            
            panel.DOFade(tempPanelFade, _uiPanelFadeTime)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });

            FadeChildren(panel.transform, fade);
        }

        public void OpenPanelMessage(RectTransform _panel, Vector3 size)
        {
            _panel.DOKill();
            _panel.gameObject.SetActive(true);
            _panel.DOScale(size, 0.2f)
                .SetEase(Ease.OutElastic);
        }

        public void CloseDontHaveMoneyPanel()
        {
            _dontHaveMoneyPanel.DOKill();
            _dontHaveMoneyPanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _dontHaveMoneyPanel.gameObject.SetActive(false);
                }));
        }
        
        public void CloseFakePanel()
        {
            _fakePanel.DOKill();
            _fakePanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _fakePanel.gameObject.SetActive(false);
                }));
        }
        
        public void CloseNotFakePanel()
        {
            _notFakePanel.DOKill();
            _notFakePanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _notFakePanel.gameObject.SetActive(false);
                }));
        }

        public void OpenSettingsPanel()
        {
            OpenPanelMessage(_settingsPanel, _settingsScale);
        }
        public void CloseSetingsPanel()
        {
            _settingsPanel.DOKill();
            _settingsPanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _settingsPanel.gameObject.SetActive(false);
                }));
        }
        
        public void OpenLootBoxPanel()
        {
            OpenPanelMessage(_lootBoxPanel, _lootBoxScale);
        }
        public void CloseLootBoxPanel()
        {
            _lootBoxPanel.DOKill();
            _lootBoxPanel.DOScale(Vector3.zero, 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete((() =>
                {
                    _lootBoxPanel.gameObject.SetActive(false);
                }));
        }

        private void FadeChildren(Transform parent, float fade)
        {
            foreach (Transform child in parent)
            {
                Image childImage = child.GetComponent<Image>();
                if (childImage != null)
                {
                    childImage.DOKill();
                    childImage.DOFade(fade, _uiPanelFadeTime);
                }

                TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
                if (childText != null)
                {
                    childText.DOKill();
                    childText.DOFade(fade, _uiPanelFadeTime);
                }
                
                CanvasGroup childCanvasGroup = child.GetComponent<CanvasGroup>();
                if (childCanvasGroup != null)
                {
                    childCanvasGroup.DOKill();
                    childCanvasGroup.DOFade(fade, _uiPanelFadeTime);
                }
                
                FadeChildren(child, fade);
            }
        }
        private void SetButtonScale(RectTransform button, Vector3 scale, float time, Ease ease)
        {
            button.DOKill();
            button.DOScale(scale, time)
                .SetEase(ease);
        }

        private void SetItemPanel(Item item)
        {
            _itemIcon.sprite = item.GetIcon();
            _itemName.text = item.GetName();
            _itemPrice.text = item.GetPrice().ToString();

            switch (item.GetRarity())
            {
                case ItemRarity.Common:
                    _itemRarity.sprite = _common;
                    break;
                case ItemRarity.Rare:
                    _itemRarity.sprite = _rare;
                    break;
                case ItemRarity.Epic:
                    _itemRarity.sprite = _epic;
                    break;
                case ItemRarity.Legendary:
                    _itemRarity.sprite = _legendary;
                    break;
                default:
                    _itemRarity.sprite = _common;
                    break;
            }
        }
    }
}
