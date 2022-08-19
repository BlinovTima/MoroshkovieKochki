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
        protected bool _isIntroCompleted;
        protected bool _isClickActionInProgress;


        public bool HasLevelInited { get; private set; }

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
            HasLevelInited = true;
        }


        protected async UniTask WaitResultsForCompleteLevel(IEnumerable<InteractionItem> interactionItems)
        {
            await UniTask.WaitUntil(() => interactionItems.All(x => x.IsCompleted));
            await UniTask.WaitUntil(() => !_isClickActionInProgress);
            CompleteLevel();
        }

        public virtual void PrepareLevelForStart()
        {
            SetupCharacter(_character);
            InputListener.OnLeftMouseButtonClick += OnClickAction;
        }

        public void CompleteLevel()
        {
            _levelTaskPopupPresenter.HidePopup().Forget();
            _popupPresenter.CloseCurrentPopup().Forget();
            _onLevelComplete.Invoke();
        }

        public async UniTask CharacterGoTo(Vector3 destinationPosition)
        {
            await _character.GoTo(destinationPosition);
        }
        
        private void OnClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
            => ClickAction(raycastHit2D, mousePosition);
        
        public abstract UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition);

        private void SetupCharacter(Character character)
        {
            _character = character;
            _character.KillAnimation();
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
            _isIntroCompleted = true;
        }

        public async UniTask ConfirmLevelTask()
        {
            await _levelTaskPopupPresenter.ConfirmLevelTask(_gameLevel);
        }

        public void Dispose()
        {
            HasLevelInited = false;
            _gameLevel.Dispose();
            Object.Destroy(_gameLevel.gameObject);
            _popupPresenter.Dispose();
            InputListener.OnLeftMouseButtonClick -= OnClickAction;
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}