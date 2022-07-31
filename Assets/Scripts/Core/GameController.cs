using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public class GameController : MonoBehaviour
    {
        [Header("Character settings")] [SerializeField]
        private Character _characterPrefab;

        [Header("Menu settings")] [SerializeField]
        private GameMenu _gameMenu;

        [SerializeField] private ScorePanel _scorePanel;
        [SerializeField] private RectTransform _popupsParent;
        [SerializeField] private WindowSwitcher _windowSwitcher;

        [Header("Levels settings")] 
        [SerializeField] private Transform _levelParent;

        private GameMenuPresenter _gameMenuPresenter;
        private int? _levelIndex;
        private GameLevelPresenter _gameLevelPresenter;
        private PopupPresenter _popupPresenter;
        private ScorePanelPresenter _scorePanelPresenter;
        private LevelTaskPopupPresenter _levelTaskPopupPresenter;
        private LevelsFabric _levelsFabric;
        private Character _character;
     
        
        private bool HasCurrentLevel => _gameLevelPresenter != null && _gameLevelPresenter.HasLevelInited;

        private void Awake()
        {
            RegisterAllSystems();
        }

        private void RegisterAllSystems()
        {
            InstantiateCharacter();
            
            _scorePanelPresenter = new ScorePanelPresenter(_scorePanel);

            _gameMenuPresenter = new GameMenuPresenter(_gameMenu,
                () =>
                {
                    if (GameContext.GameIsFinished)
                        ResetGame();
                    
                    StartNextLevel().Forget();
                });
            InputListener.OnEscKeyGet += _gameMenuPresenter.SwitchMenu;
            
            _levelsFabric = new LevelsFabric(_levelParent,
                _popupsParent,
                _character,
                () => StartNextLevel().Forget());

            _levelsFabric.InitLevels();
            _gameMenuPresenter.ShowMenu();
        }

        private void SwitchGameMenu()
        {
            _gameMenuPresenter.SwitchMenu();
        }

        private async UniTask StartNextLevel()
        {
            GameContext.RemoveGameState(GameState.Play);
            GameContext.AddGameState(GameState.CutScene);

            if (HasCurrentLevel)
                await _gameLevelPresenter.PlayOutro();

            await _windowSwitcher.SwitchWindow(() =>
            {
                if (!TryLoadLevel())
                {
                    GameContext.GameIsFinished = true;
                    _gameMenuPresenter.ShowMenu();
                    GameContext.RemoveGameState(GameState.CutScene);
                }
            });

            if (!GameContext.GameIsFinished && HasCurrentLevel)
            {
                await _gameLevelPresenter.PlayIntro();
                
                GameContext.RemoveGameState(GameState.CutScene);
                GameContext.AddGameState(GameState.Play);
            }
        }

        private bool TryLoadLevel()
        {
            _gameMenuPresenter.HideMenu();

            if (HasCurrentLevel)
            {
                _character.transform.SetParent(transform.root, false);
                _gameLevelPresenter.Dispose();
            }

            if (_levelsFabric.GetNextLevel(out var gameLevelPresenter))
            {
                _gameLevelPresenter = gameLevelPresenter;
                _gameLevelPresenter.PrepareLevelForStart();
                return true;
            }
            
            _gameLevelPresenter = null;
            GameContext.GameIsFinished = true;
            return false;
            
        }

        private void ResetGame()
        {
            GameContext.Reset();
            _levelsFabric.InitLevels();
        }

        private void InstantiateCharacter()
        {
            var characterInstance = Instantiate(_characterPrefab);
            _character = characterInstance;
        }
        
        private void OnApplicationQuit()
        {
            _scorePanelPresenter.Dispose();
            InputListener.OnEscKeyGet -= _gameMenuPresenter.SwitchMenu;
        }
    }
}