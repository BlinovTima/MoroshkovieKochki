using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace MoroshkovieKochki
{
    public abstract class GameLevel : MonoBehaviour, IDisposable
    {
        [Header("Data for task popup")]
        [ResizableTextArea]
        [SerializeField] private string _description;
        [SerializeField] private string _buttonLabel;
        
        [Header("Character")]
        [SerializeField] private Transform _characterParent;
        [SerializeField] protected Transform _initialPosition;
        
        private IGameLevelEventReceiver _gameLevelEventReceiver;
        protected float _characterXBound;

        public string Description => _description;
        public string ButtonLabel => _buttonLabel;
        public Transform CharacterParent => _characterParent;
        public Transform InitialPosition => _initialPosition;

        public virtual void Init(IGameLevelEventReceiver eventReceiver, 
            float characterXBound)
        {
            _characterXBound = characterXBound;
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