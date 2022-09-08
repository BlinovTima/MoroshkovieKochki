using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class BushItem : InteractionItem
    {
        [SerializeField] public bool ShouldGather;
        
        [Header("OnClick settings")]
        [SerializeField] private OutlineAnimation _outlineAnimation;

        [Header("Gather elements settings")]
        [SerializeField] private float _delayBetweenSwitchOff = 0.3f;
        [SerializeField] private float _switchOffTime = 1f;
        [SerializeField] public List<SpriteRenderer> _berries;
      

        private int _delayBetweenSwitchOffMilliseconds;


        private void Awake()
        {
            _berries.ForEach(x => x.gameObject.SetActive(true));
            _delayBetweenSwitchOffMilliseconds = (int)(_delayBetweenSwitchOff * 1000);
        }

        public override async void OnClick<TClickResult>(TClickResult value)
        {
            if(IsCompleted)
                return;


            if (value is BooleanClickResult clickResult) 
                IsRightAdvice = ShouldGather == clickResult.ButtonClickValue;

            IsCompleted = IsRightAdvice;
            
            await _outlineAnimation.ShowOutline(IsRightAdvice);
            
            if(IsRightAdvice)
                GameContext.AddScoreValue(1);
            
            if (IsRightAdvice && ShouldGather)
            {
                foreach (var spriteRenderer in _berries)
                {
                    Animate(spriteRenderer).Forget();
                    await UniTask.Delay(_delayBetweenSwitchOffMilliseconds);
                }
            }
            else if (!IsRightAdvice)
            {
                await _outlineAnimation.HideOutline();
            }
        }

        private async UniTask Animate(SpriteRenderer berrie)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => berrie.color.a, x => SetAlpha(berrie, x), 0f, _switchOffTime));
            sequence.AppendCallback(() => berrie.gameObject.SetActive(false));

            await sequence.AsyncWaitForCompletion();
        }
        
        private void SetAlpha(SpriteRenderer berrie, float alpha)
        {
            var color = berrie.color;
            color.a = alpha;
            berrie.color = color;
        }
    }
}