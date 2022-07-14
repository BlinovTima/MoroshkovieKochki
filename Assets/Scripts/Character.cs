using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.7f;
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public async UniTask GoTo(Vector3 newPosition)
        {
            var duration = CalculateMoveTime(newPosition);
           await transform.DOMove(newPosition, duration).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }

        private float CalculateMoveTime(Vector3 newPosition)
        {
            var distance = Vector3.Distance(transform.position, newPosition);
            return distance / _speed;
        }
    }
}