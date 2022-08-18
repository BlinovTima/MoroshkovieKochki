using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class BerriesLevelPresenter : GameLevelPresenter
    {
        public override void PrepareLevelForStart()
        {
            base.PrepareLevelForStart();
            _character.SetAnimationPreset(CharacterAnimationPreset.Box);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
        }

        public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            if (!_isIntroCompleted)
                return;

            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();

            var isMouseInsidePopup = _popupPresenter.IsPointInPopup(mousePosition);
            if (isMouseInsidePopup)
                return;

            var item = raycastHit2D.collider.GetComponent<BushItem>();
            var road = raycastHit2D.collider.GetComponent<Road>();

            if (!item || _popupPresenter.NeedCloseCurrentPopup(item))
                _popupPresenter.CloseCurrentPopup();

            if (road)
            {
                await _character.GoTo(raycastHit2D.point)
                    .AttachExternalCancellation(_cancellationToken.Token);
            }

            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Уже нельзя");
                }
                else
                {
                    await _character.GoTo(item.CharacterInteractionPoint.position)
                        .AttachExternalCancellation(_cancellationToken.Token);
                    await _popupPresenter.ShowPopUp(item)
                        .AttachExternalCancellation(_cancellationToken.Token);

                    await UniTask.WaitUntil(() => !_popupPresenter.IsPopupOpen);

                    if (item.IsCompleted && item.ShouldGather)
                    {
                        await _character.PlayGather().AttachExternalCancellation(_cancellationToken.Token);
                        _character.PlayIdle();
                    }
                }
            }
        }
    }
}