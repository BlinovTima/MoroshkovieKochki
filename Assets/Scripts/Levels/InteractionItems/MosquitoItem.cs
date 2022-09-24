using Cysharp.Threading.Tasks;
using PathCreation;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class MosquitoItem : InteractionItem
    {
        private const float _minDistance = 0.1f;
        
        [Header("OnClick settings")]
        [SerializeField] private MosquitoHouse _mosquitoHouse;
        [Space(10)]
        [SerializeField] private PathCreator _introPath;
        [SerializeField] private PathCreator _loopPath;
        [SerializeField] private Transform _outroPathTransform;
        [SerializeField] private Transform _mosquitoContainer;
        [SerializeField] private SpriteRenderer _mosquitoSpriteRenderer;
        [SerializeField] private Animator _mosquitoAnimator;
        [SerializeField] private float _flySpeed;
        [SerializeField] private AudioSource _audioSource;

        private bool _isGoingLeftCahce;
        private Character _character;
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");

        private void Awake()
        {
            _outroPathTransform.position = Vector3.zero;
        }

        public void Init(Character character)
        {
            _character = character;
        }
        
        public override void OnClick<TClickResult>(TClickResult value)
        {
            if (IsCompleted)
                return;

            if (value is MosquitoClickResult clickResult)
                IsRightAdvice = _mosquitoHouse == clickResult.MosquitoHouse;

            IsCompleted = IsRightAdvice;

            if (IsRightAdvice)
                GameContext.AddScoreValue(1);
        }

        public async UniTask FlyOutro(Vector3 finishPosition)
        { 
            var bezierPath = new BezierPath(new[] { _mosquitoContainer.position, finishPosition },
               false, 
               PathSpace.xy);
            
            var path = new VertexPath(bezierPath, _outroPathTransform);
            
            await FlyToPoint(path);
            _audioSource.Pause();
            _mosquitoAnimator.SetBool(IsIdle, true);
        }

        public async UniTask FlyIntro()
        {
            var path = _introPath.path;
            var startPosition = path.GetClosestPointOnPath(_mosquitoContainer.position);
            _mosquitoContainer.position = startPosition;
            _audioSource.Play();
            
            await FlyToPoint(path);
        }

        public async UniTask FlyLoop()
        {
            var distance = 0f;
            var path = _loopPath.path;
            var startPosition = path.GetClosestPointOnPath(_mosquitoContainer.position);
            _mosquitoContainer.position = startPosition;
           
            while (!IsCompleted)
            {
                distance += _flySpeed * Time.deltaTime;
                var newPosition = path.GetPointAtDistance(distance);
                SetSideOrientation(newPosition);
                SetOrderInLayer();
                _mosquitoContainer.position = newPosition;
                 
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private async UniTask FlyToPoint(VertexPath vertexPath)
        {
            var distance = 0f;
            var finalDistance = vertexPath.length;
            
            while (finalDistance - distance > _minDistance)
            {
                var newPosition = vertexPath.GetPointAtDistance(distance);
                SetSideOrientation(newPosition);
                _mosquitoContainer.position = newPosition;
                distance += _flySpeed * Time.deltaTime;

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void SetOrderInLayer()
        {
            if(!_mosquitoSpriteRenderer)
                return;
            
            var mosquitoWorldPosition = _mosquitoContainer.TransformPoint(_mosquitoContainer.position);
            if (mosquitoWorldPosition.y > _character.Position.y)
                _mosquitoSpriteRenderer.sortingOrder = _character.SortingOrder - 1;
            else
                _mosquitoSpriteRenderer.sortingOrder = _character.SortingOrder + 1;
        }
        
        private void SetSideOrientation(Vector3 newPosition)
        {
            var isGoingLeft = _mosquitoContainer.position.x > newPosition.x;

            if (isGoingLeft == _isGoingLeftCahce)
                return;

            _isGoingLeftCahce = isGoingLeft;

            var localScale = _mosquitoContainer.localScale;
            localScale.x *= -1;
            _mosquitoContainer.localScale = localScale;
        }
    }
}