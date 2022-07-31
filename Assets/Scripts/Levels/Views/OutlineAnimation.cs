using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace MoroshkovieKochki
{
    [RequireComponent(typeof(Transform))]
    public class OutlineAnimation : MonoBehaviour
    {
        [Header("Colors")] [SerializeField] private Color _completeColor;
        [SerializeField] private Color _wrongColor;

        [Header("Animation settings")] [SerializeField]
        private Vector3 _outlineScale;

        [SerializeField] private float _showDuration = 1f;
        [SerializeField] private float _hideDuration = 0.2f;
        [SerializeField] private Ease _ease = Ease.Linear;

        private Transform _transform;
        private Material _material;
        private bool _isActivated;
        private Vector3 _initialOutlineScale;
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _material = GetComponent<Renderer>().material;
            _isActivated = false;
            _initialOutlineScale = _transform.localScale;
        }

        public async UniTask HideOutline()
        {
            if (!_isActivated)
                return;
            
            await _transform.DOScale(_initialOutlineScale, _hideDuration)
                .SetEase(Ease.Linear)
                .OnComplete(()=> _isActivated = false)
                .AsyncWaitForCompletion();
        }
        
        public async UniTask ShowOutline(bool isComplete)
        {
            var outlineColor = isComplete ? _completeColor : _wrongColor;
            _material.SetColor("_Color", outlineColor);
            
            await HideOutline();
            
            await _transform.DOScale(_outlineScale, _showDuration)
                .SetEase(_ease)
                .OnComplete(()=> _isActivated = true)
                .AsyncWaitForCompletion();
        }

        [Button("Test animation")]
        private async void TestOutline()
        {
            await ShowOutline(true);
        }
    }
}