using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


namespace MoroshkovieKochki
{
    public abstract class ItemPopup : MonoBehaviour
    {
        [SerializeField] private RectTransform _boundsRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _switchTime = 0.4f;
          
        private Sequence _sequence;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public RectTransform BoundsRect => _boundsRect;
        
        
        public async UniTask Show(Vector3 screenPivotPoint)
        {
            transform.position = screenPivotPoint;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _switchTime));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }
        
        public async UniTask Hide()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _switchTime));
            _sequence.AppendCallback(() => gameObject.SetActive(false));
            _sequence.SetAutoKill(true);
            
            await _sequence.AsyncWaitForKill();
        }
    }
}