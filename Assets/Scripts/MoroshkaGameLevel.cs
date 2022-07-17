using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class MoroshkaGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        
        public override async UniTask PlayIntro()
        {
           await _character.GoTo(_introPosition.position);
        }

        public override async UniTask PlayOutro()
        {
            await _character.GoTo(_outroPosition.position);
        }

        protected override void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D)
        {
            if(raycastHit2D.collider.GetComponent<Road>())
                _character.GoTo(raycastHit2D.point).Forget();
        }
    }
}