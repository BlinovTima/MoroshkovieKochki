using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class FootprintsGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        [SerializeField] private List<FootprintsItem> _footprintsItems;
        private IGameLevelEventReceiver _eventReceiver;

        public List<FootprintsItem> FootprintsItems => _footprintsItems;
     
        public override void Init(IGameLevelEventReceiver eventReceiver, 
            float characterXBound)
        {
            _eventReceiver = eventReceiver;
            base.Init(eventReceiver, characterXBound);
        }
        
        public override async UniTask PlayIntro()
        { 
            await _eventReceiver.ConfirmLevelTask(); 
            await _eventReceiver.CharacterGoTo(_introPosition.position);
        }

        public override async UniTask PlayOutro()
        {
            await _eventReceiver.CharacterGoTo(_outroPosition.position);
        }
    }
}