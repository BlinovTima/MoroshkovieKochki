using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class GatherItem : InteractionItem
    {
        [SerializeField] public bool ShouldSayYes;
        
        [Header("OnClick settings")]
        [SerializeField] private OutlineAnimation _outlineAnimation;
        
        [Header("Elements settings")]
        [SerializeField] private float _switchOffTime = 1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        
        public override async void OnClick<TClickResult>(TClickResult value)
        {
            if(IsCompleted)
                return;
            
            if (value is BooleanClickResult clickResult) 
                IsRightAdvice = ShouldSayYes == clickResult.ButtonClickValue;

            IsCompleted = IsRightAdvice;
            
            await _outlineAnimation.ShowOutline(IsRightAdvice);

            if(IsRightAdvice)
                GameContext.AddScoreValue(1);
            
            if (IsRightAdvice && ShouldSayYes)
            {
                await _outlineAnimation.HideOutline();
                Animate(_spriteRenderer).Forget();
            }
            else if (!IsRightAdvice)
            {
                await _outlineAnimation.HideOutline();
            }
        }
        
        private async UniTask Animate(SpriteRenderer sr)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => sr.color.a, x => SetAlpha(sr, x), 0f, _switchOffTime));
            sequence.AppendCallback(() => sr.gameObject.SetActive(false));

            await sequence.AsyncWaitForCompletion();
        }
        
        private void SetAlpha(SpriteRenderer sr, float alpha)
        {
            var color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }
}