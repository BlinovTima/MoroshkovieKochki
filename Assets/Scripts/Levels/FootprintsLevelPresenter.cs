using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class FootprintsLevelPresenter : GameLevelPresenter
    {
        private FootprintsGameLevel _footprintsLevel;

        public override void Init(GameLevel gameLevel, Action onLevelComplete, Character character, PopupPresenter popupPresenter,
            LevelTaskPopupPresenter levelTaskPopupPresenter)
        {
            _footprintsLevel = gameLevel as FootprintsGameLevel;
            base.Init(gameLevel, onLevelComplete, character, popupPresenter, levelTaskPopupPresenter);
        }
        
        public override void PrepareLevelForStart()
        {
            base.PrepareLevelForStart();
            _character.SetAnimationPreset(CharacterAnimationPreset.Default);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
        }

        public override async UniTask PlayIntro()
        {
            await base.PlayIntro();
            AnimalsQuestionQueueProcess(_footprintsLevel.FootprintsItems).Forget();
        }

        private async UniTask AnimalsQuestionQueueProcess(List<FootprintsItem> interactionItemsAction)
        {
            var completedItems = new List<FootprintsItem>(interactionItemsAction);
            
            foreach (var footprintsItem in interactionItemsAction)
            {
                await _popupPresenter.ShowPopUp(footprintsItem)
                    .AttachExternalCancellation(_cancellationToken.Token);

                await UniTask.WaitUntil(() => completedItems.Any(x =>
                {
                    var completed = x.IsCompleted;
                    if (completed)
                        completedItems.Remove(x);
                   
                    return completed;
                }));
            }
        }

        public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            var isMouseInsidePopup = _popupPresenter.IsPointInPopup(mousePosition);
            if (isMouseInsidePopup)
                return;
            
            var item = raycastHit2D.collider.GetComponent<FootprintsItem>();
            if (item)
            {
                if (item.IsCompleted)
                {
                    Debug.LogError("Уже нельзя");
                }
                else
                {
                    var popup = (FootprintsPopup) _popupPresenter.CurrentPopup;
                    item.OnClick(new FootprintClickResult(){FootprintAnimal = popup.RightAnswer});
                }
            }
        }
    }
}