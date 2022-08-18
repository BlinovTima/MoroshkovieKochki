using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MoroshkovieKochki
{
    public sealed class FootprintsGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        [SerializeField] private List<FootprintsItem> _footprintsItems;

        public List<FootprintsItem> FootprintsItems => _footprintsItems;
     
        public override void Init(IGameLevelEventReceiver eventReceiver, 
            float characterXBound)
        {
            base.Init(eventReceiver, characterXBound);
            
            _initialPosition.MovePositionBehindFrustrum(_characterXBound);
            _outroPosition.MovePositionBehindFrustrum(_characterXBound);
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