using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class MoroshkaGameLevel : GameLevel
    {
        [SerializeField] private Transform _introPosition;
        [SerializeField] private Transform _outroPosition;
        
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public override async UniTask PlayIntro()
        {
           await _character.GoTo(_introPosition.position);
        }

        public override async UniTask PlayOutro()
        {
            await _character.GoTo(_outroPosition.position);
        }

        protected override void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            ProduceClickAction(raycastHit2D, mousePosition).Forget();
        }
        
        private async UniTask ProduceClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();

            var isMouseInsidePopup = _popupPresenter.IsPointInPopup(mousePosition);
            if (isMouseInsidePopup)
                return;
            
            var item = raycastHit2D.collider.GetComponent<InteractionItem>();
            var road = raycastHit2D.collider.GetComponent<Road>();

            if (!item || _popupPresenter.NeedCloseCurrentPopup(item))
               _popupPresenter.CloseCurrentPopup();

            if (road)
                await _character.GoTo(raycastHit2D.point).AttachExternalCancellation(_cancellationToken.Token);

            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Уже нельзя");
                }
                else
                {
                    await _character.GoTo(item.CharacterInteractionPoint.position).AttachExternalCancellation(_cancellationToken.Token);
                    await _popupPresenter.ShowPopUp(item).AttachExternalCancellation(_cancellationToken.Token);
                }
            }
        }
    }
}