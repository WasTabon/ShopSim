using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ShopSim.Scripts.Sellers
{
    public class Seller : MonoBehaviour
    {
        public event Action OnItemComplete;

        [SerializeField] private Image _itemIcon;
        [SerializeField] private float _uiDialogueScaleTimeIn;
        [SerializeField] private float _uiDialogueScaleTimeOut;
        
        [SerializeField] private AudioClip _greetingSound;
        [SerializeField] private AudioClip[] _insideWalkSounds;
        [SerializeField] private AudioClip[] _outsideWalkSounds;

        private Item _item;
        private BuyController _buyController;

        private RectTransform _dialogue;
        private Vector3 _originalScale;

        private AudioSource _audioSource;
        private Animator _animator;

        private Transform[] _positions;
        private Transform _queueFirst;
        private int _currentTargetIndex;
        private bool _initialized;
        private bool _needMoveQueue;
        private float _speed = 2f;
        private float _stopDistance = 0.1f;
        private Transform _currentTarget;

        private bool _isWalking;
        private float _walkSoundTimer;
        private float _walkSoundInterval = 0.6f;
        private bool _cameToQueuePosition; 
            
        public bool isInsideShop;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _audioSource = GetComponentInChildren<AudioSource>();
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

        public Item GetItem()
        {
            return _item;
        }

        private void Update()
        {
            if (!_initialized)
                return;

            if (_currentTargetIndex < _positions.Length)
            {
                HandleMove(_positions[_currentTargetIndex]);
            }

            if (_currentTargetIndex >= _positions.Length && _cameToQueuePosition == false)
            {
                _cameToQueuePosition = true;
                Debug.Log("Came to position", this);
            }
            else if (_needMoveQueue && _cameToQueuePosition)
            {
                HandleMove(_currentTarget);
            }

            if (_currentTarget == null && _currentTargetIndex >= _positions.Length)
            {
                HandleIdle();
            }
            
            HandleWalkSound();
        }

        public void ItemProccesed()
        {
            OnItemComplete?.Invoke();
        }

        public void HandleOnSeller()
        {
            _dialogue.DOScale(_originalScale, _uiDialogueScaleTimeIn)
                .SetEase(Ease.OutBack);

            PlaySound(_greetingSound);
            _buyController.SetCurrentSeller(this);
        }

        public void SetMoveToTarget(Transform target)
        {
            _currentTarget = target;
            _needMoveQueue = true;
        }

        public void HandleMove(Transform target)
        {
            if (target == null) return;

            Vector3 direction = Vector3.one;

            if (!_cameToQueuePosition)
            {
                Debug.Log($"Walk to shop", this);
                direction = (target.position - transform.position).normalized;
            }
            else if (_needMoveQueue && _cameToQueuePosition)
            {
                Debug.Log($"Start new moving to {_currentTarget.gameObject.name}", this);
                direction = (_currentTarget.position - transform.position).normalized;   
            }

            transform.position += direction * _speed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.001f)
                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsIdle", false);

            _isWalking = true;

            if (Vector3.Distance(transform.position, target.position) < _stopDistance)
            {
                if (!_cameToQueuePosition)
                {
                    _currentTargetIndex++;
                }
                else if (_needMoveQueue || _cameToQueuePosition)
                {
                    _needMoveQueue = false;
                    _currentTarget = null;
                }
            }
        }

        private void HandleIdle()
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsIdle", true);
            _isWalking = false;
            _walkSoundTimer = 0f;
        }

        private void HandleWalkSound()
        {
            if (_isWalking)
            {
                _walkSoundTimer -= Time.deltaTime;
                if (_walkSoundTimer <= 0f)
                {
                    if (isInsideShop)
                    {
                        int randomSound = Random.Range(0, _insideWalkSounds.Length);
                        PlaySound(_insideWalkSounds[randomSound], 0.3f);
                    }
                    else
                    {
                        int randomSound = Random.Range(0, _outsideWalkSounds.Length);
                        PlaySound(_outsideWalkSounds[randomSound], 0.03f);
                    }
                    
                    _walkSoundTimer = _walkSoundInterval;
                }
            }
        }

        private void PlaySound(AudioClip audioClip, float volume = 0.5f)
        {
            _audioSource.PlayOneShot(audioClip, volume);
        }

        private T FindChildWithTag<T>(string tag) where T : Component
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
