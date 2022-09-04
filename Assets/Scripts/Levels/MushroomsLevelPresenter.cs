using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class MushroomsLevelPresenter : GameLevelPresenter
    {
        
        public override void PrepareLevelForStart()
        {
            _character.SetAnimationPreset(CharacterAnimationPreset.Basket);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
            base.PrepareLevelForStart();
        }
        
        public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            if (!_isIntroCompleted)
                return;
            
            _isClickActionInProgress = true;
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();

            var isMouseInsidePopup = _popupPresenter.IsPointInPopup(mousePosition);
            if (isMouseInsidePopup)
                return;
            
            var item = raycastHit2D.collider.GetComponent<GatherItem>();
            var road = raycastHit2D.collider.GetComponent<Road>();

            if (!item || _popupPresenter.NeedCloseCurrentPopup(item))
                _popupPresenter.CloseCurrentPopup().Forget();

            if (road)
                await _character.GoTo(raycastHit2D.point)
                    .AttachExternalCancellation(_cancellationToken.Token);

            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Здесь больше делать нечего");
                }
                else
                {
                    await _character.GoTo(item.CharacterInteractionPoint.position)
                        .AttachExternalCancellation(_cancellationToken.Token);
                    
                    _character.PlayThinking().Forget();
                   
                    await _popupPresenter.ShowPopUp(item)
                        .AttachExternalCancellation(_cancellationToken.Token);
                    
                    await UniTask.WaitUntil(() => !_popupPresenter.IsPopupOpen);

                    if (item.IsCompleted && item.ShouldSayYes)
                        await _character.PlayGather().AttachExternalCancellation(_cancellationToken.Token);
                    else if (!item.IsCompleted && item.ShouldSayYes)
                        await _character.PlayNo().AttachExternalCancellation(_cancellationToken.Token);

                    _character.PlayIdle();
                }
            }
            
            if(_cancellationToken.IsCancellationRequested)
                _character.PlayIdle();
            
            _isClickActionInProgress = false;
        }
    }
}