using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace MoroshkovieKochki
{
    public abstract class GameLevel : MonoBehaviour, IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private RectTransform _characterParent;
        [SerializeField] private Transform _initialPosition;
        protected Character _character;
        private Action _onLevelComplete;

        public virtual void Init(Action onLevelComplete, Character character)
        {
#if UNITY_EDITOR
            _onLevelComplete = onLevelComplete;
#endif
            SetupCharacter(character);
            InputListener.OnLeftMouseButtonClick += OnLeftMouseButtonClick;
        }

        private void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D)
        {
            Debug.Log(raycastHit2D.collider.gameObject.name);
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