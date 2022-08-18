using System;
using System.Linq;
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
        
        protected IGameLevelEventReceiver _eventReceiver;
        protected float _characterXBound;
        private InteractionItem[] _interactionItems = new FootprintsItem[] { };
        
        public string Description => _description;
        public string ButtonLabel => _buttonLabel;
        public Transform CharacterParent => _characterParent;
        public Transform InitialPosition => _initialPosition;

        public InteractionItem[] InteractionItems
        {
            get
            {
                if(!_interactionItems.Any())
                    _interactionItems = gameObject.GetComponentsInChildren<InteractionItem>();

                return _interactionItems;
            }
        }
        
        public virtual void Init(IGameLevelEventReceiver eventReceiver, 
            float characterXBound)
        {
            _characterXBound = characterXBound;
            _eventReceiver = eventReceiver;
        }

        public virtual async UniTask PlayIntro()
        {
            await UniTask.Yield();
        }

        public virtual async UniTask PlayOutro()
        {
            await UniTask.Yield();
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
            _eventReceiver.CompleteLevel();
        }
    }
}