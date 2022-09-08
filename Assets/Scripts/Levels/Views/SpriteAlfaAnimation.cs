using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAlfaAnimation : OutlineAnimation
    {
        [Header("Colors")] 
        [SerializeField] private Color _completeColor;
        [SerializeField] private Color _wrongColor; 
        
        [Header("Animation")]        
        [SerializeField] private float _showDuration = 1f;
        [SerializeField] private float _hideDuration = 0.2f;
        [SerializeField] private Ease _ease = Ease.OutElastic;

        private SpriteRenderer _spriteRenderer;
        private bool _isActivated;
        private Color _initialColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _initialColor = Color.white;
            _initialColor.a = 0f;
            _spriteRenderer.color = _initialColor;
        }

        public override async UniTask HideOutline()
        {
            if (!_isActivated)
                return;
            
            await _spriteRenderer.DOColor(_initialColor, _hideDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isActivated = false)
                .AsyncWaitForCompletion();
        }
        
        public override async UniTask ShowOutline(bool isComplete)
        {
            var outlineColor = isComplete ? _completeColor : _wrongColor;
            
            await HideOutline();
            
            gameObject.SetActive(true);
            await _spriteRenderer.DOColor(outlineColor, _showDuration)
                .SetEase(_ease)
                .OnComplete(()=> _isActivated = true)
                .AsyncWaitForCompletion();
        }

        public override async UniTask FlickOutline(bool isComplete)
        {
            await ShowOutline(isComplete);
            await HideOutline();
        }
    }
}