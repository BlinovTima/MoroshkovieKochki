using System;
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

        [SerializeField] private HudPanel hudPanel;
        [SerializeField] private RectTransform _popupsParent;
        [SerializeField] private WindowSwitcher _windowSwitcher;

        [Header("Levels settings")] 
        [SerializeField] private Transform _levelParent;

        private GameMenuPresenter _gameMenuPresenter;
        private int? _levelIndex;
        private GameLevelPresenter _gameLevelPresenter;
        private HudPresenter _hudPresenter;
        private LevelTaskPopupPresenter _levelTaskPopupPresenter;
        private LevelsFabric _levelsFabric;
        private Character _character;
     
        
        private bool HasCurrentLevel => _gameLevelPresenter != null && _gameLevelPresenter.HasLevelInited;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InstantiateCharacter();
            RegisterAllSystems();
        }

        private void RegisterAllSystems()
        {
            _gameMenuPresenter = new GameMenuPresenter(_gameMenu, StartNewGame);
            InputListener.OnEscKeyGet += OpenGameMenu;

            _hudPresenter = new HudPresenter(hudPanel, OpenGameMenu, StartNewGame);

            _levelsFabric = new LevelsFabric(_levelParent,
                _popupsParent,
                _character,
                () => StartNextLevel().Forget());

            _levelsFabric.InitLevels();
            _gameMenuPresenter.ShowMenu();
        }

        private void OpenGameMenu()
        {
            _gameMenuPresenter.SwitchMenu();
        }

        private void StartNewGame()
        {
            ResetGame();
            StartNextLevel().Forget();
        }

        private async UniTask StartNextLevel()
        {
            GameContext.RemoveGameState(GameState.Play);
            GameContext.AddGameState(GameState.CutScene);

            if (HasCurrentLevel)
                await _gameLevelPresenter.PlayOutro();
            
            await _windowSwitcher.SwitchWindow(LoadLevel);

            if (!GameContext.GameIsFinished && HasCurrentLevel)
            {
                GameContext.AddLevelPassed();
                await _gameLevelPresenter.PlayIntro();
                
                GameContext.RemoveGameState(GameState.CutScene);
                GameContext.AddGameState(GameState.Play);
            }
        }

        private void LoadLevel()
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
                return;
            }
            
            _gameLevelPresenter = null;
            GameContext.GameIsFinished = true;
            _gameMenuPresenter.ShowMenu();
            GameContext.RemoveGameState(GameState.CutScene);
        }

        private void ResetGame()
        {
            Time.timeScale = 1f;
            _character.KillAnimation();
            _character.transform.SetParent(transform.root, false);
            _gameLevelPresenter?.Dispose();
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
            _hudPresenter.Dispose();
            InputListener.OnEscKeyGet -= OpenGameMenu;
        }
    }
}