using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            _character.SetAnimationPreset(CharacterAnimationPreset.Newspaper);
            WaitResultsForCompleteLevel(_gameLevel.InteractionItems).Forget();
            base.PrepareLevelForStart();
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

                await UniTask.WaitUntil(() => _activeMosquito.IsCompleted);
                await _popupPresenter.CloseCurrentPopup();
                await UniTask.WaitUntil(() => !_hasActiveMosquito);
            }
        }

        public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            if (!_isIntroCompleted || _isClickActionInProgress)
                return;
            
            _isClickActionInProgress = true; 
            
            var item = raycastHit2D.collider.GetComponentInParent<House>();
            
            if (item && _activeMosquito)
            {
                _activeMosquito.OnClick(new MosquitoClickResult(){MosquitoHouse = item.MosquitoHouse});
               
                item.FlickOutline(_activeMosquito.IsCompleted);
                
                if(_activeMosquito.IsCompleted)
                {
                    _character.PlayHit().Forget();
                    
                    await _activeMosquito.FlyOutro(item.transform.position);
                    _activeMosquito = null;
                    _hasActiveMosquito = false;
                }
                else
                {
                   await _character.PlayNo().AttachExternalCancellation(_cancellationToken.Token);
                }
                
                _character.PlayIdle();
            }
            
            if(_cancellationToken.IsCancellationRequested)
                _character.PlayIdle();
            
            _isClickActionInProgress = false; 
        }
    }
}