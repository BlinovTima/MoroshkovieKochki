using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MoroshkovieKochki
{
    public sealed class MosquitoGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        [SerializeField] private List<MosquitoItem> _mosquitoItems;

        public List<MosquitoItem> MosquitoItems => _mosquitoItems;

        public override void Init(IGameLevelEventReceiver eventReceiver, float characterXBound)
        {
            base.Init(eventReceiver, characterXBound);
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