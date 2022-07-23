using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace MoroshkovieKochki
{
    public abstract class GameLevel : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _characterParent;
        [SerializeField] private Transform _initialPosition;
        
        private IGameLevelEventReceiver _gameLevelEventReceiver;


        public Transform CharacterParent => _characterParent;
        public Transform InitialPosition => _initialPosition;
        

        public virtual void Init(IGameLevelEventReceiver eventReceiver)
        {
            _gameLevelEventReceiver = eventReceiver;
        }

        public virtual async UniTask PlayIntro()
        {
            await UniTask.Yield();
        }

        public virtual async UniTask PlayOutro()
        {
            await UniTask.Yield();
        }
        
        public virtual void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            Debug.Log($"Collider name = {raycastHit2D.collider.gameObject.name} Mouse pos = {mousePosition}");
        }

        
        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
           
        }

        [Button("Complete Level")]
        public void CompleteLevel()
        {
            _gameLevelEventReceiver.LevelComplete();
        }
    }
}