using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;


namespace MoroshkovieKochki
{
    public sealed class BerriesGameLevel : GameLevel
    {
        [Header("Positions")]
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        
        public override void Init(IGameLevelEventReceiver eventReceiver, 
            float characterXBound)
        {
            _eventReceiver = eventReceiver;
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