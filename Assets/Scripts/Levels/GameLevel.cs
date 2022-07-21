using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace MoroshkovieKochki
{
    public abstract class GameLevel : MonoBehaviour, IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private Transform _characterParent;
        [SerializeField] private Transform _initialPosition;
        protected Character _character;
        private Action _onLevelComplete;
        protected PopupPresenter _popupPresenter;

        public virtual void Init(Action onLevelComplete, 
            Character character,
            PopupPresenter popupPresenter)
        {
            _popupPresenter = popupPresenter;
            _onLevelComplete = onLevelComplete;
            
            SetupCharacter(character);
            InputListener.OnLeftMouseButtonClick += OnLeftMouseButtonClick;
            
        }

        protected virtual void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            Debug.Log($"Collider name = {raycastHit2D.collider.gameObject.name} Mouse pos = {mousePosition}");
        }
        
        public virtual async UniTask PlayIntro()
        {
            await UniTask.Yield();
        }

        public virtual async UniTask PlayOutro()
        {
            await UniTask.Yield();
        }

        private void SetupCharacter(Character character)
        {
            _character = character;
            _character.transform.SetParent(_characterParent, true);
            _character.SetPosition(_initialPosition.position);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _character = null;
            InputListener.OnLeftMouseButtonClick -= OnLeftMouseButtonClick;
        }

        [Button("Complete Level")]
        public void CompleteLevel()
        {
            _onLevelComplete.Invoke();
        }
    }
}