﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class GatherPopupData : PopupData
    {
        [Header("OnClick settings")]
        [SerializeField] public bool ShouldGather;
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

        public override async void OnClick(bool value)
        {
            if(IsCompleted)
                return;
            
            IsCompleted = true;
            
            var isRightAdvice = value == ShouldGather;

            await _outlineAnimation.ShowOutline(isRightAdvice);
            
            if (isRightAdvice && ShouldGather)
            {
                foreach (var spriteRenderer in _berries)
                {
                    Animate(spriteRenderer).Forget();
                    await UniTask.Delay(_delayBetweenSwitchOffMilliseconds);
                }
            }
        }

        private async UniTask Animate(SpriteRenderer berrie)
        {
            var _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => berrie.color.a, x => SetAlpha(berrie, x), 0f, _switchOffTime));
            _sequence.AppendCallback(() => berrie.gameObject.SetActive(false));

            await _sequence.AsyncWaitForCompletion();
        }
        
        private void SetAlpha(SpriteRenderer berrie, float alpha)
        {
            var color = berrie.color;
            color.a = alpha;
            berrie.color = color;
        }
    }
}