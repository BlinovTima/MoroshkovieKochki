using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoroshkovieKochki
{
    public class LevelTaskPopup : PopupGeneric
    {
        [SerializeField] private TMP_Text _desription;
        [SerializeField] private TMP_Text _buttonLabel;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private RectTransform _boundsRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _switchTime = 0.4f;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _hideEase;
        
        private Sequence _sequence;
        
        public bool IsTaskConfirmed { get; private set; }
        
        public void Init(string description, string buttonLabel)
        {
            gameObject.SetActive(false);
            _desription.text = description;
            _buttonLabel.text = buttonLabel;
            
            _confirmButton.onClick.AddListener(() =>
            {
                IsTaskConfirmed = true;
                _confirmButton.onClick.RemoveAllListeners();
                Hide().Forget();
            });
        }
        
        public async UniTask Show()
        {
            IsTaskConfirmed = false;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _switchTime)
                .SetEase(_showEase));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }

        public async UniTask Hide()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _switchTime)
                .SetEase(_hideEase));
            _sequence.AppendCallback(() => gameObject.SetActive(false));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }
    }
}