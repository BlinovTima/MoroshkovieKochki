using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class MushroomsGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
            
            
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