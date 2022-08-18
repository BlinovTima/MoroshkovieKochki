using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MoroshkovieKochki
{
    public abstract class GameLevelPresenter : IGameLevelEventReceiver, IDisposable
    {
        protected GameLevel _gameLevel;
        private Action _onLevelComplete;
        protected PopupPresenter _popupPresenter;
        private LevelTaskPopupPresenter _levelTaskPopupPresenter;
        protected CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        protected Character _character;


        public bool HasLevelInited => _gameLevel != null;

        public virtual void Init(
            GameLevel gameLevel,
            Action onLevelComplete,
            Character character,
            PopupPresenter popupPresenter,
            LevelTaskPopupPresenter levelTaskPopupPresenter)
        {
            _levelTaskPopupPresenter = levelTaskPopupPresenter;
            _character = character;
            _popupPresenter = popupPresenter;
            _onLevelComplete = onLevelComplete;

            _gameLevel = gameLevel;
            _gameLevel.Init(this, _character.BoundsXSize);
        }


        protected async UniTask WaitResultsForCompleteLevel(IEnumerable<InteractionItem> interactionItems)
        {
            await UniTask.WaitUntil(() => interactionItems.All(x => x.IsCompleted));
            CompleteLevel();
        }

        public virtual void PrepareLevelForStart()
        {
            SetupCharacter(_character);
            InputListener.OnLeftMouseButtonClick += ClickAction;
        }

        public void CompleteLevel()
        {
            _levelTaskPopupPresenter.HidePopup().Forget();
            _popupPresenter.CloseCurrentPopup();
            _onLevelComplete.Invoke();
        }

        public async UniTask CharacterGoTo(Vector3 destinationPosition)
        {
            await _character.GoTo(destinationPosition);
        }

        public abstract void ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition);

        private void SetupCharacter(Character character)
        {
            _character = character;
            _character.SetScale(ScaleType.Default);
            _character.PlayIdle();
            _character.transform.SetParent(_gameLevel.CharacterParent, false);
            _character.SetPosition(_gameLevel.InitialPosition.position);
        }

        public virtual async UniTask PlayOutro()
        {
            await _gameLevel.PlayOutro();
        }

        public virtual async UniTask PlayIntro()
        {
            await _gameLevel.PlayIntro();
        }

        public async UniTask ConfirmLevelTask()
        {
            await _levelTaskPopupPresenter.ConfirmLevelTask(_gameLevel);
        }

        public void Dispose()
        {
            _gameLevel.Dispose();
            Object.Destroy(_gameLevel.gameObject);
            _popupPresenter.Dispose();
            InputListener.OnLeftMouseButtonClick -= ClickAction;
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}