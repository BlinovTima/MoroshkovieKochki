using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


namespace MoroshkovieKochki
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

        public async UniTask PlayGameIntro(Action loadLevelCallback)
        {
            SwitchScreen(loadLevelCallback);
            await _sequence.AsyncWaitForCompletion();
        }
        
        public async UniTask SwitchWindow(Action loadLevelCallback)
        {
            FadeVolume().Forget();
            SwitchScreen(loadLevelCallback);
            await _sequence.AsyncWaitForCompletion();
        }

        private async UniTaskVoid FadeVolume()
        {
            var fadeTime = _switchTime / 3;
            await AudioManager.SetMusicVolumeDown(fadeTime);
            AudioManager.SmoothMusicVolumeUp(fadeTime).Forget();
        }

        private void SwitchScreen(Action loadLevelCallback)
        {
            var halfTime = _switchTime / 2;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _switchScreen.alpha = 0;
            _switchScreen.gameObject.SetActive(true);

            _sequence.Append(DOTween.To(() => _switchScreen.alpha, x => _switchScreen.alpha = x, 1f, halfTime));
            _sequence.AppendCallback(loadLevelCallback.Invoke);
            _sequence.Append(DOTween.To(() => _switchScreen.alpha, x => _switchScreen.alpha = x, 0f, halfTime));
            _sequence.AppendCallback(() => _switchScreen.gameObject.SetActive(false));
        }
    }
}