using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSim.Scripts.Sellers
{
    public class Seller : MonoBehaviour
    {
        public event Action OnItemComplete;

        [SerializeField] private Image _itemIcon;
        
        [SerializeField] private float _uiDialogueScaleTimeIn;
        [SerializeField] private float _uiDialogueScaleTimeOut;

        private Item _item;
        
        private BuyController _buyController;
        
        private RectTransform _dialogue;
        private Vector3 _originalScale;
        
        private Animator _animator;

        private Transform[] _positions;
        private Transform _queueFirst;
        private int _currentTargetIndex;
        private bool _initialized;
        private bool _needMoveQueue;
        private float _speed = 2f;
        private float _stopDistance = 0.1f;
        private Transform _currentTarget;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _dialogue = FindChildWithTag<RectTransform>("Dialogue");
            _originalScale = _dialogue.localScale;
            _dialogue.localScale = Vector3.zero;
            
            _buyController = FindObjectOfType<BuyController>();
        }

        public void Initialize(Transform movePosition1, Transform movePosition2, Transform movePosition3, Transform movePosition4, Transform queuePosition, Item item)
        {
            _positions = new Transform[] { movePosition1, movePosition2, movePosition3, movePosition4, queuePosition };
            _currentTargetIndex = 0;
            _item = item;

            _itemIcon.sprite = item.GetIcon();
            
            _initialized = true;
            
            if (_positions.Length > 0)
                transform.forward = (_positions[0].position - transform.position).normalized;
        }

        private void Update()
        {
            if (!_initialized)
                return;

            if (_currentTargetIndex < _positions.Length)
            {
                HandleMove(_positions[_currentTargetIndex]);
            }
            else if (_needMoveQueue)
            {
                HandleMove(_currentTarget);
            }
            else
            {
                HandleIdle();
            }
        }

        public void HandleMove(Transform target)
        {
            if (target == null) return;
            
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * _speed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.001f)
                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsIdle", false);

                if (Vector3.Distance(transform.position, target.position) < _stopDistance)
                {
                    if (!_needMoveQueue)
                    {
                        _currentTargetIndex++;
                    }
                    else
                    {
                        _currentTarget = null;
                        _needMoveQueue = false;
                    }
                }
        }

        public void SetMoveToTarget(Transform target)
        {
            _currentTarget = target;
            _needMoveQueue = true;
        }

        private void HandleIdle()
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsIdle", true);
        }

        public void HandleFinishQueue()
        {
            _dialogue.DOScale(_originalScale, _uiDialogueScaleTimeIn)
                .SetEase(Ease.OutBack);
            
            _buyController.SetCurrentSeller(this);
        }

        public void ItemProccesed()
        {
            OnItemComplete?.Invoke();
        }
        
        private T FindChildWithTag<T>( string tag) where T : Component
        {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag(tag))
                {
                    return child.GetComponent<T>();
                }
            }
            return null;
        }
    }
}
