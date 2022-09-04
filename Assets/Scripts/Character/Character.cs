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
        [SerializeField] private Transform _skeletonTransform;
        [SerializeField] private SkeletonAnimationEvents _skeletonAnimationEvents;

        [Header("Scale setup")]
        [SerializeField] private Vector3 _defaultScale = new Vector3(0.45f, 0.45f, 0.45f);
        [SerializeField] private Vector3 _titleScale = new Vector3(0.7f, 0.7f, 0.7f);
        
        private Sequence _sequence;
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
        
        public void SetAnimationPreset(CharacterAnimationPreset preset) => 
            _animationPreset = Animations.GetPreset(preset);

        public void SetPosition(Vector3 position) => 
            transform.localPosition = position;

        public void PlaySay() => 
            SetAnimation(_animationPreset.Say).Forget();
        
        public void PlayIdle() => 
            SetAnimation(_animationPreset.Idle).Forget();
        
        public async UniTask PlayHello() => 
            await SetAnimation(_animationPreset.Hello, false);
        
        public async UniTask PlayHit()
        {
            await SetAnimation(_animationPreset.Hit, false);
            PlayIdle();
        }

        public async UniTask PlayThinking()
        {
            await SetAnimation(_animationPreset.ThinkStart, false);
            await SetAnimation(_animationPreset.ThinkLoop, true);
        }

        public async UniTask PlayFinishThinking() =>
            await SetAnimation(_animationPreset.ThinkFinish, false);
        
        public async UniTask PlayGather() => 
            await SetAnimation(_animationPreset.Take, false);

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
            if(string.IsNullOrEmpty(animationName))
                return;
            
            var track = _skeletonAnimation.state.SetAnimation(0, animationName, isLoop);
            await new WaitForSpineAnimationComplete(track);
        }

        public void SetOrientationRight() => 
            SetOrientation(1);

        private void SetSideOrientation(Vector3 newPosition)
        {
            var scale = transform.position.x > newPosition.x ? -1 : 1;
            SetOrientation(scale);
        }

        private void SetOrientation(float scale)
        {
            var scaleClamped = Mathf.Clamp(scale, -1, 1);
            var localScale = transform.localScale;
            localScale.x = scaleClamped;
            transform.localScale = localScale;
        }

        private float CalculateMoveTime(Vector3 newPosition)
        {
            var distance = Vector3.Distance(transform.position, newPosition);
            return distance / _speed;
        }

        public void SetupFootstepSound(AudioClip footstepSound)
        {
            _skeletonAnimationEvents.SetupAudioClip(footstepSound);
        }
    }
}