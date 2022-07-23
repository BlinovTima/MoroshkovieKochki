using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MoroshkovieKochki
{
    public class GameLevelPresenter : IGameLevelEventReceiver, IDisposable
    {
        private GameLevel _gameLevel;
        private readonly Action _onLevelComplete;
        private readonly PopupPresenter _popupPresenter;
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private Character _character;

        public bool HasInitedLevel => _gameLevel != null;
        
        public GameLevelPresenter(
            Action onLevelComplete, 
            Character character,
            PopupPresenter popupPresenter)
        {
            _character = character;
            _popupPresenter = popupPresenter;
            _onLevelComplete = onLevelComplete;
            
        }

        public void InitLevel(GameLevel gameLevel)
        {
            _gameLevel = gameLevel;
            _gameLevel.Init(this);
            SetupCharacter(_character);
            InputListener.OnLeftMouseButtonClick += OnLeftMouseButtonClick;
        }
        
        public void LevelComplete()
        {
            _onLevelComplete.Invoke();
        }

        public async UniTask CharacterGoTo(Vector3 destinationPosition)
        {
            await _character.GoTo(destinationPosition);
        }

        private void SetupCharacter(Character character)
        {
            _character = character;
            _character.transform.SetParent(_gameLevel.CharacterParent, false);
            _character.SetPosition(_gameLevel.InitialPosition.position);
        }
        
        private void OnLeftMouseButtonClick(RaycastHit2D raycastHit2D, Vector3 mousePosition)
        {
            _gameLevel.OnLeftMouseButtonClick(raycastHit2D, mousePosition);
        }

        public void Dispose()
        {
            _gameLevel.Dispose();
            Object.Destroy(_gameLevel.gameObject);
            _popupPresenter.Dispose();
            InputListener.OnLeftMouseButtonClick -= OnLeftMouseButtonClick;
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        public async UniTask PlayOutro()
        {
            await _gameLevel.PlayOutro();
        }

        public async UniTask PlayIntro()
        {
            await _gameLevel.PlayIntro();
        }
        
        public async UniTask GatherClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
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