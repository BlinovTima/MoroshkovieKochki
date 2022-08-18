using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed partial class Character : MonoBehaviour
    {
        [SerializeField] private float _minAnimationDistance = 0.1f;
        [SerializeField] private float _speed = 0.7f;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _skeletonTransform;

        [Header("Scale setup")]
        [SerializeField] private Vector3 _defaultScale = new Vector3(0.45f, 0.45f, 0.45f);
        [SerializeField] private Vector3 _titleScale = new Vector3(0.7f, 0.7f, 0.7f);
        
        private Sequence _sequence;
        private bool _isGoingLeftCache;
        private AnimationPreset _animationPreset;
  
        
        public float BoundsXSize => _meshRenderer.bounds.size.x;
        public int SortingOrder => _meshRenderer.sortingOrder;
        public Vector3 Position => transform.position;

        public void SetScale(ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.Default:
                    _skeletonTransform.localScale = _defaultScale;
                    break;
                case ScaleType.Title:
                    _skeletonTransform.localScale = _titleScale;
                    break;
            }
        }
        
        public void SetAnimationPreset(CharacterAnimationPreset preset)
        {
            _animationPreset = Animations.GetPreset(preset);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void PlaySay() => SetAnimation(_animationPreset.Say).Forget();
        public void PlayIdle() => SetAnimation(_animationPreset.Idle).Forget();

        public async UniTask PlayGather() => await SetAnimation(_animationPreset.Take, false);

        public void KillAnimation()
        {
            _sequence?.Kill();
            _sequence = null;
        }
        
        public async UniTask GoTo(Vector3 newPosition)
        {
            newPosition.z = 0;
            
            if (Vector3.Distance(transform.localPosition, newPosition) < _minAnimationDistance)
                return;

            SetSideOrientation(newPosition);
            SetAnimation(_animationPreset.Walk).Forget();

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            var duration = CalculateMoveTime(newPosition);

            _sequence.Append(transform.DOLocalMove(newPosition, duration).SetEase(Ease.Linear));
            _sequence.AppendCallback(() => SetAnimation(_animationPreset.Idle).Forget());
            _sequence.SetAutoKill(true);

            await _sequence.AsyncWaitForKill();
        }

        private async UniTask SetAnimation(string animationName, bool isLoop = true)
        {
            var track = _skeletonAnimation.state.SetAnimation(0, animationName, isLoop);
            await new WaitForSpineAnimationComplete(track);
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