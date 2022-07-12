using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


namespace DefaultNamespace
{
    public class WindowSwitcher : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _switchScreen;
        [SerializeField] private float _switchTime;
        
        private Sequence _sequence;

        private void Awake()
        {
            gameObject.SetActive(true);
        }

        public async UniTask SwitchWindow(Action loadLevelCallback)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            var halfTime = _switchTime / 2;
            _sequence.AppendCallback(() =>
            {
                _switchScreen.alpha = 0;
                _switchScreen.gameObject.SetActive(true);
            });
            
            _sequence.Append(DOTween.To(() => _switchScreen.alpha, x => _switchScreen.alpha = x, 1f, halfTime));
            
            _sequence.AppendCallback(loadLevelCallback.Invoke);
            
            _sequence.Append(DOTween.To(() => _switchScreen.alpha, x => _switchScreen.alpha = x, 0f, halfTime));
            
            _sequence.AppendCallback(() => _switchScreen.gameObject.SetActive(false));

            await _sequence.AsyncWaitForCompletion();
        }
    }
}