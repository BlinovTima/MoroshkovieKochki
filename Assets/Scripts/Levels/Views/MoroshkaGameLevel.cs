using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class MoroshkaGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        
        private IGameLevelEventReceiver _eventReceiver;

        public override void Init(IGameLevelEventReceiver eventReceiver)
        {
            _eventReceiver = eventReceiver;
            base.Init(eventReceiver);

            var interactionItems = gameObject.GetComponentsInChildren<InteractionItem>();
            WaitResultsForCompleteLevel(interactionItems, eventReceiver.LevelComplete).Forget();
        }

        private async UniTask WaitResultsForCompleteLevel(InteractionItem[] interactionItems, Action onLevelComplete)
        {
            await UniTask.WaitUntil(() => interactionItems.All(x => x.IsCompleted));
            onLevelComplete.Invoke();
        }
        
        public override async UniTask PlayIntro()
        {
           await _eventReceiver.CharacterGoTo(_introPosition.position);
        }

        public override async UniTask PlayOutro()
        {
            await _eventReceiver.CharacterGoTo(_outroPosition.position);
        }

        public override void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            _eventReceiver.GatherClickAction(raycastHit2D, mousePosition).Forget();
        }
        
    }
}