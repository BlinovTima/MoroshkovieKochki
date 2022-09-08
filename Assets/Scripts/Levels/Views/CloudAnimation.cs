using System.Threading;
using Cysharp.Threading.Tasks;
using PathCreation;
using UnityEngine;

public sealed class CloudAnimation : MonoBehaviour
{
    private const float _minDistance = 0.1f;
    
    [SerializeField] private PathCreator _path;
    [SerializeField] private Transform _cloudContainer;
    [SerializeField] private float _flySpeed;
    [SerializeField] private bool _startFromRandomPosition = true;

    private CancellationTokenSource _cancellationToken;
    private float _distance;

    private void Start()
    {
        _cancellationToken = new CancellationTokenSource();
        var path = _path.path;
        
        if (_startFromRandomPosition)
            _distance = Random.Range(0f, path.length);
        
        Fly(path).Forget();
    }

    private void OnDestroy()
    {
        _cancellationToken.Cancel();
    }

    private async UniTaskVoid Fly(VertexPath vertexPath)
    {
        var finalDistance = vertexPath.length;
            
        while (true)
        {
            var newPosition = vertexPath.GetPointAtDistance(_distance);
            _cloudContainer.position = newPosition;
            _distance += _flySpeed * Time.deltaTime;

            if (finalDistance - _distance < _minDistance) 
                _distance = 0f;

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cancellationToken.Token);
        }
    }
}
