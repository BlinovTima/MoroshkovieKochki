using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public class GameController : MonoBehaviour
    {
        [Header("Character settings")]
        [SerializeField] private Character _characterPrefab;

        [Header("Menu settings")]
        [SerializeField] private GameMenu _gameMenu;
        [SerializeField] private ScorePanel _scorePanel;
        [SerializeField] private RectTransform _popupsParent;
        [SerializeField] private WindowSwitcher _windowSwitcher;
        
        [Header("Levels settings")]
        [SerializeField] private Transform _levelParent;
        [SerializeField] private GameLevel _startScreen;
        [SerializeField] private GameLevel _finalScreen;
        [SerializeField] private List<GameLevel> _gameLevels;

        private GameMenuPresenter _gameMenuPresenter;
        private int? _levelIndex;
        private GameLevelPresenter _gameLevelPresenter;
        private PopupPresenter _popupPresenter;
        private ScorePanelPresenter _scorePanelPresenter;

        private GameLevel GetNextLevel()
        {
            if (!_levelIndex.HasValue)
            {
                _levelIndex = 0;
                return _startScreen;
            }
            
            if (_levelIndex.Value < _gameLevels.Count)
            {
                var levelIndex = _levelIndex.Value;
                _levelIndex += 1;
                return _gameLevels[levelIndex];
            }

            return _finalScreen;
        }
        
        private void Awake()
        {
            RegisterAllSystems();
        }

        private void RegisterAllSystems()
        {
            _scorePanelPresenter = new ScorePanelPresenter(_scorePanel);
            
            InstantiateCharacter();

            _gameMenuPresenter = new GameMenuPresenter(_gameMenu,
                () => StartNextLevel().Forget(),
                () => ResetAndPlayAgain().Forget());
            InputListener.OnEscKeyGet += _gameMenuPresenter.SwitchMenu;

            
            _popupPresenter = new PopupPresenter(_popupsParent);

            _gameLevelPresenter = new GameLevelPresenter(() => StartNextLevel().Forget(),
                _characterPrefab,
                _popupPresenter);
            
            _gameMenuPresenter.ShowMenu();
        }

        private void SwitchGameMenu()
        {
            _gameMenuPresenter.SwitchMenu();
        }
        
        private void InstantiateCharacter()
        {
            var characterInstance = Instantiate(_characterPrefab, transform.root);
            _characterPrefab = characterInstance;
        }

        private async UniTask StartNextLevel()
        {
            GameContext.RemoveGameState(GameState.Play);
            GameContext.AddGameState(GameState.CutScene);

            var hasCurrentLevel = _gameLevelPresenter.HasInitedLevel;
            
            if (hasCurrentLevel)
                await _gameLevelPresenter.PlayOutro();
            
            await _windowSwitcher.SwitchWindow(() => LoadLevel(hasCurrentLevel));
            await _gameLevelPresenter.PlayIntro();

            GameContext.RemoveGameState(GameState.CutScene);
            GameContext.AddGameState(GameState.Play);
        }

        private void LoadLevel(bool hasCurrentLevel)
        {
            _gameMenuPresenter.HideMenu();

            if (hasCurrentLevel)
                _gameLevelPresenter.Dispose();
            
            var nextLevelPrefab = GetNextLevel();
            var nextLevelInstance =  Instantiate(nextLevelPrefab, _levelParent);
            _gameLevelPresenter.InitLevel(nextLevelInstance);
        }

        private async UniTask ResetAndPlayAgain()
        {
            GameContext.Reset();
        }

        private void OnApplicationQuit()
        {
            _scorePanelPresenter.Dispose();
            _gameLevelPresenter.Dispose();
            InputListener.OnEscKeyGet -= _gameMenuPresenter.SwitchMenu;
        }
    }
    
    
}