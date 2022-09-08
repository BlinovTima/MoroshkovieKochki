using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Renderer))]
    public class OutlineShaderAnimation : OutlineAnimation
    {
        [Header("Colors")] 
        [SerializeField] private Color _completeColor;
        [SerializeField] private Color _wrongColor;

        [Header("Animation settings")] 
        [SerializeField] private Vector3 _outlineScale;

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

        public override async UniTask HideOutline()
        {
            if (!_isActivated)
                return;
            
            await _transform.DOScale(_initialOutlineScale, _hideDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _isActivated = false;
                    gameObject.SetActive(false);
                })
                .AsyncWaitForCompletion();
        }
        
        public override async UniTask ShowOutline(bool isComplete)
        {
            var outlineColor = isComplete ? _completeColor : _wrongColor;
            _material.SetColor("_Color", outlineColor);
            
            await HideOutline();
            
            gameObject.SetActive(true);
            await _transform.DOScale(_outlineScale, _showDuration)
                .SetEase(_ease)
                .OnComplete(()=> _isActivated = true)
                .AsyncWaitForCompletion();
        }

        public override async UniTask FlickOutline(bool isComplete)
        {
            Debug.LogError("Not implement FlickOutline");
            await UniTask.Yield();
        }
    }
}