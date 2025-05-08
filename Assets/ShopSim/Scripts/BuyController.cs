using DG.Tweening;
using ShopSim.Scripts.Sellers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSim.Scripts
{
    public class BuyController : MonoBehaviour
    {
        [SerializeField] private float _uiPanelFadeTime;
        [SerializeField] private float _uiButtonScaleTimeIn;
        [SerializeField] private float _uiButtonScaleTimeOut;

        [SerializeField] private Image _checkPanel;
        [SerializeField] private RectTransform _uiButtonCheck;

        [SerializeField] private Sprite _common;
        [SerializeField] private Sprite _rare;
        [SerializeField] private Sprite _epic;
        [SerializeField] private Sprite _legendary;
        
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Image _itemRarity;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemPrice;

        private Vector3 _buyScale;
        private Vector3 _checkScale;
        private Vector3 _denyScale;
        
        private Seller _currentSeller;

        private void Start()
        {
            _checkScale = _uiButtonCheck.localScale;
            
            _uiButtonCheck.localScale = Vector3.zero;

            float tempFadeTime = _uiPanelFadeTime;
            _uiPanelFadeTime = 0;
            
            SetPanelFade(_checkPanel, 0);
            _checkPanel.gameObject.SetActive(false);
            _uiPanelFadeTime = tempFadeTime;
        }

        public void SetCurrentSeller(Seller seller)
        {
            _currentSeller = seller;
            SetButtonScale(_uiButtonCheck, _checkScale, _uiButtonScaleTimeIn, Ease.OutElastic);
            SetItemPanel(_currentSeller.GetItem());
        }
        
        public void Buy()
        {
            if (_currentSeller != null)
            {
                _currentSeller.ItemProccesed();
                _currentSeller = null;
                SetPanelFade(_checkPanel, 0);
                SetButtonScale(_uiButtonCheck, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
                _checkPanel.gameObject.SetActive(false);
            }
        }

        public void OpenCheckPanel()
        {
            _checkPanel.gameObject.SetActive(true);
            SetPanelFade(_checkPanel, 1);
        }

        public void SetPanelFade(Image panel, float fade)
        {
            float tempPanelFade = 0;
            panel.DOKill();
            if (fade == 1)
                tempPanelFade = 0.45f;
            else if (fade == 0)
                tempPanelFade = 0;
            
            panel.DOFade(tempPanelFade, _uiPanelFadeTime);

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
