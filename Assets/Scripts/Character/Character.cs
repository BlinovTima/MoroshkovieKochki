using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private float _minAnimationDistance = 0.1f;
        [SerializeField] private float _speed = 0.7f;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private MeshRenderer _meshRenderer;
        private Sequence _sequence;
        private bool _isGoingLeftCache;
        private AnimationPreset _animationPreset;

        public float BoundsXSize => _meshRenderer.bounds.size.x;
        public int SortingOrder => _meshRenderer.sortingOrder;
        public Vector3 Position => transform.position;

        public void SetAnimationPreset(CharacterAnimationPreset preset)
        {
            _animationPreset = Animations.GetPreset(preset);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public async UniTask GoTo(Vector3 newPosition)
        {
            newPosition.z = 0;
            
            if (Vector3.Distance(transform.localPosition, newPosition) < _minAnimationDistance)
                return;

            SetSideOrientation(newPosition);
            SetAnimation(_animationPreset.Walk);

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            var duration = CalculateMoveTime(newPosition);

            _sequence.Append(transform.DOLocalMove(newPosition, duration).SetEase(Ease.Linear));
            _sequence.AppendCallback(() => SetAnimation(_animationPreset.Idle));
            _sequence.SetAutoKill(true);

            await _sequence.AsyncWaitForKill();
        }

        private void SetAnimation(string animationName)
        {
            _skeletonAnimation.AnimationName = animationName;
        }

        private void SetSideOrientation(Vector3 newPosition)
        {
            var isGoingLeft = transform.position.x > newPosition.x;

            if (isGoingLeft == _isGoingLeftCache)
                return;

            _isGoingLeftCache = isGoingLeft;

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