using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.7f;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        
        
        private Sequence _sequnce;
        private bool _isGoingLeftCahce;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public async UniTask GoTo(Vector3 newPosition)
        {
            SetSideOrientation(newPosition);
            SetAnimation("walk");
            
            _sequnce?.Kill();
            _sequnce = DOTween.Sequence();
            var duration = CalculateMoveTime(newPosition);
            
            _sequnce.Append(transform.DOMove(newPosition, duration).SetEase(Ease.Linear));
            _sequnce.AppendCallback(() => SetAnimation("idle"));
            
            _sequnce.SetAutoKill(true);
            
            await _sequnce.AsyncWaitForKill();
        }

        private void SetAnimation(string animationName)
        {
            _skeletonAnimation.AnimationName = animationName;
        }
        
        private void SetSideOrientation(Vector3 newPosition)
        {
            var isGoingLeft = transform.position.x > newPosition.x;
            
            if(isGoingLeft == _isGoingLeftCahce)
                return;
            
            _isGoingLeftCahce = isGoingLeft;
            
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        private float CalculateMoveTime(Vector3 newPosition)
        {
            var distance = Vector3.Distance(transform.position, newPosition);
            return distance / _speed;
        }
    }
}