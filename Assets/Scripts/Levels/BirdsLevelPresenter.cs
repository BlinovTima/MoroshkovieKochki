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
            _character.SetAnimationPreset(CharacterAnimationPreset.Default);
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

            if (!item || _popupPresenter.NeedCloseCurrentPopup(item))
            {
                AudioManager.StopSpeech();
                _popupPresenter.CloseCurrentPopup().Forget();
            }

            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Уже нельзя");
                }
                else
                {
                    await _popupPresenter.ShowPopUp(item)
                        .AttachExternalCancellation(_cancellationToken.Token);
                    
                    await UniTask.WaitUntil(() => !_popupPresenter.IsPopupOpen);

                    if (item.IsCompleted)
                        AudioManager.PlaySpeech(item.CorrectChoiceAudio);
                    else
                        AudioManager.PlaySpeech(item.IncorrectChoiceAudio);

                    if (item.IsCompleted && item.ShouldSayYes)
                        await _character.PlayHello().AttachExternalCancellation(_cancellationToken.Token);
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