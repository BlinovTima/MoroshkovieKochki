﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


namespace MoroshkovieKochki
{
    public abstract class ItemPopup : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _switchTime = 0.4f;
          
        private Sequence _sequence;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        
        public async UniTask Show(Vector3 screenPivotPoint)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.AppendCallback(() =>
            {
                transform.position = screenPivotPoint;
                _canvasGroup.alpha = 0;
                _canvasGroup.gameObject.SetActive(true);
            });
            
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _switchTime));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }
        
        public async UniTask Hide()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _switchTime));
            _sequence.AppendCallback(() => _canvasGroup.gameObject.SetActive(false));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }
    }
}