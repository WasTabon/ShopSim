using DG.Tweening;
using ShopSim.Scripts.Sellers;
using UnityEngine;

namespace ShopSim.Scripts
{
    public class BuyController : MonoBehaviour
    {
        [SerializeField] private float _uiButtonScaleTimeIn;
        [SerializeField] private float _uiButtonScaleTimeOut;
        
        [SerializeField] private RectTransform _uiButtonBuy;
        [SerializeField] private RectTransform _uiButtonCheck;
        [SerializeField] private RectTransform _uiButtonDeny;

        private Vector3 _buyScale;
        private Vector3 _checkScale;
        private Vector3 _denyScale;
        
        private Seller _currentSeller;

        private void Start()
        {
            _buyScale = _uiButtonBuy.localScale;
            _checkScale = _uiButtonCheck.localScale;
            _denyScale = _uiButtonDeny.localScale;

            _uiButtonBuy.localScale = Vector3.zero;
            _uiButtonCheck.localScale = Vector3.zero;
            _uiButtonDeny.localScale = Vector3.zero;
        }

        public void SetCurrentSeller(Seller seller)
        {
            _currentSeller = seller;
            SetBuyButtonsScaleIn();
        }

        private void SetBuyButtonsScaleIn()
        {
            SetButtonScale(_uiButtonBuy, _buyScale, _uiButtonScaleTimeIn, Ease.OutElastic);
            SetButtonScale(_uiButtonCheck, _checkScale, _uiButtonScaleTimeIn, Ease.OutElastic);
            SetButtonScale(_uiButtonDeny, _denyScale, _uiButtonScaleTimeIn, Ease.OutElastic);
        }
        private void SetBuyButtonsScaleOut()
        {
            SetButtonScale(_uiButtonBuy, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
            SetButtonScale(_uiButtonCheck, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
            SetButtonScale(_uiButtonDeny, Vector3.zero, _uiButtonScaleTimeOut, Ease.Flash);
        }

        private void SetButtonScale(RectTransform button, Vector3 scale, float time, Ease ease)
        {
            button.DOKill();
            button.DOScale(scale, time)
                .SetEase(ease);
        }

        public void Buy()
        {
            if (_currentSeller != null)
            {
                SetBuyButtonsScaleOut();
                _currentSeller.ItemProccesed();
                _currentSeller = null;
            }
        }
    }
}
