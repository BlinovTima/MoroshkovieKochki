using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class BirdsLevelPresenter : GameLevelPresenter
    {
        public override void PrepareLevelForStart()
        {
            base.PrepareLevelForStart();
            _character.SetAnimationPreset(CharacterAnimationPreset.Default);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
        }

        public override async void ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();

            var isMouseInsidePopup = _popupPresenter.IsPointInPopup(mousePosition);
            if (isMouseInsidePopup)
                return;
            
            var item = raycastHit2D.collider.GetComponent<InteractionItem>();

            if (!item || _popupPresenter.NeedCloseCurrentPopup(item))
                _popupPresenter.CloseCurrentPopup();

            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Уже нельзя");
                }
                else
                {
                    await _popupPresenter.ShowPopUp(item).AttachExternalCancellation(_cancellationToken.Token);
                }
            }
        }
    }
}