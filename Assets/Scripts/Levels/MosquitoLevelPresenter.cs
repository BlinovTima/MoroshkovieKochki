using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class MosquitoLevelPresenter : GameLevelPresenter
    {
        private MosquitoGameLevel _mosquitoGameLevel;
        private MosquitoItem _activeMosquito;
        private bool _hasActiveMosquito;

        public override void Init(GameLevel gameLevel, Action onLevelComplete, Character character, PopupPresenter popupPresenter,
            LevelTaskPopupPresenter levelTaskPopupPresenter)
        {
            _mosquitoGameLevel = gameLevel as MosquitoGameLevel;
            _mosquitoGameLevel.MosquitoItems.ForEach(x => x.Init(character));
            base.Init(gameLevel, onLevelComplete, character, popupPresenter, levelTaskPopupPresenter);
        }
        
        public override void PrepareLevelForStart()
        {
            base.PrepareLevelForStart();
            _character.SetAnimationPreset(CharacterAnimationPreset.Newspaper);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
        }
        
        public override async UniTask PlayIntro()
        {
            await base.PlayIntro();
            AnimalsQuestionQueueProcess(_mosquitoGameLevel.MosquitoItems).Forget();
        }

        private async UniTask AnimalsQuestionQueueProcess(List<MosquitoItem> interactionItemsAction)
        {
            foreach (var mosquitoItem in interactionItemsAction)
            {
                await mosquitoItem.FlyIntro().AttachExternalCancellation(_cancellationToken.Token);
                
                mosquitoItem.FlyLoop().Forget();

                _activeMosquito = mosquitoItem;
                _hasActiveMosquito = true;
                
                await _popupPresenter.ShowPopUp(mosquitoItem)
                    .AttachExternalCancellation(_cancellationToken.Token);

                await UniTask.WaitUntil(() => !_hasActiveMosquito);
            }
        }

        public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            var item = raycastHit2D.collider.GetComponentInParent<MosquitoItem>();
            
            if (item && _activeMosquito)
            {
                item.OnClick(new MosquitoClickResult(){MosquitoHouse = _activeMosquito.MosquitoHouse});
                
                if(_activeMosquito.IsCompleted)
                {
                    await _activeMosquito.FlyOutro(raycastHit2D.transform.position);
                    _activeMosquito = null;
                    _hasActiveMosquito = false;
                }
            }
        }
    }
}