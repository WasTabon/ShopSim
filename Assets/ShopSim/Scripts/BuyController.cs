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
        
        [SerializeField] private Image _checkPanel;
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
        
        private Vector3 _buyScale;
        private Vector3 _checkScale;
        private Vector3 _denyScale;

        private bool _isNightPanelOpened;
        
        private Seller _currentSeller;

        private void Start()
        {
            _itemSlots = new List<Image>();
            _checkScale = _uiButtonCheck.localScale;
            
            _uiButtonCheck.localScale = Vector3.zero;

            float tempFadeTime = _uiPanelFadeTime;
            _uiPanelFadeTime = 0;
            
            SetPanelFade(_checkPanel, 0);
            _checkPanel.gameObject.SetActive(false);
            _uiPanelFadeTime = tempFadeTime;
        }

        private void Update()
        {
            if (DayTimeController.Instance.IsNight && _sellersManager.sellersQueue.Count <= 0 && !_isNightPanelOpened)
            {
                OpenNightPanel();
            }
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
                    _earnedMoneyCount += _currentSeller.GetItem().GetPrice();
                    _earnedMoneyText.text = _earnedMoneyCount.ToString();
                    _itemsSoldCount++;
                    
                    SetPanelFade(_checkPanel, 0);
                    SetButtonScale(_uiButtonCheck, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
                    _checkPanel.gameObject.SetActive(false);
                    _currentSeller.ItemProccesed(true);
                    _currentSeller = null;
                }
            }
        }

        public void Verify()
        {
            if (_moneyCount - 50 >= 0)
            {
                if (_currentSeller != null)
                {
                    if (_currentSeller.GetItem().GetFake())
                    {
                        _moneyCount -= 50;
                    }
                }   
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
            _moneyCount += _earnedMoneyCount;
            _earnedMoneyCount = 0;
            _itemsSoldCount = 0;
            foreach (Image itemSlot in _itemSlots)
            {
                Destroy(itemSlot);
            }
            _itemSlots.Clear();
            
            SetPanelFade(_itemsListPanel, 0, SetDayAferSold);
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
