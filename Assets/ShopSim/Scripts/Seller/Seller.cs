using DG.Tweening;
using UnityEngine;

namespace ShopSim.Scripts.Seller
{
    public class Seller : MonoBehaviour
    {
        private RectTransform _dialogue;
        private Vector3 _originalScale;
        
        private Animator _animator;

        private Transform[] _positions;
        private Transform _queueFirst;
        private int _currentTargetIndex;
        private bool _initialized;
        private float _speed = 2f;
        private float _stopDistance = 0.1f;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _dialogue = FindChildWithTag<RectTransform>("Dialogue");
            _originalScale = _dialogue.localScale;
            _dialogue.localScale = Vector3.zero;
        }

        public void Initialize(Transform movePosition1, Transform movePosition2, Transform queuePosition)
        {
            _positions = new Transform[] { movePosition1, movePosition2, queuePosition };
            _currentTargetIndex = 0;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized)
                return;

            if (_currentTargetIndex < _positions.Length)
            {
                HandleMove(_positions[_currentTargetIndex]);
            }
            else
            {
                HandleIdle();
            }
        }

        private void HandleMove(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * _speed * Time.deltaTime;

            if (direction != Vector3.zero)
                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsIdle", false);

            if (Vector3.Distance(transform.position, target.position) < _stopDistance)
            {
                _currentTargetIndex++;
            }
        }

        private void HandleIdle()
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsIdle", true);
        }

        public void ShowDialogue()
        {
            _dialogue.DOScale(_originalScale, 0.5f)
                .SetEase(Ease.OutBack);
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
