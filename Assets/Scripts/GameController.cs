using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameMenu _gameMenu;
        [SerializeField] private Character _character;
        [SerializeField] private WindowSwitcher _windowSwitcher;
        
        [Header("Levels settings")]
        [SerializeField] private Transform _levelParent;
        [SerializeField] private GameLevel _startScreen;
        [SerializeField] private GameLevel _finalScreen;
        [SerializeField] private List<GameLevel> _gameLevels;

        private GameMenuPresenter _gameMenuPresenter;
        private int? _levelIndex;
        private GameLevel _currentLevel;

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
            _gameMenuPresenter = new GameMenuPresenter(_gameMenu,
                () => StartNextLevel().Forget(),
                () => ResetAndPlayAgain().Forget());

            InputListener.OnEscKeyGet += _gameMenuPresenter.SwitchMenu;

            _gameMenuPresenter.ShowMenu();
        }

        private async UniTask StartNextLevel()
        {
            GameContext.AddGameState(GameState.CutScene);

            var hasCurrentLevel = _currentLevel != null;
            
            if (hasCurrentLevel)
                await _currentLevel.PlayOutro();
            
            await _windowSwitcher.SwitchWindow(() =>
            {
                _gameMenuPresenter.HideMenu();

                if (hasCurrentLevel)
                    Destroy(_currentLevel.gameObject);

                var nextLevel = GetNextLevel();
                _currentLevel = Instantiate(nextLevel, _levelParent);
                _currentLevel.Init(() => StartNextLevel().Forget(), _character);
            });

            await _currentLevel.PlayIntro();
            
            GameContext.RemoveGameState(GameState.CutScene);
        }
        
        private async UniTask ResetAndPlayAgain()
        {
            GameContext.Reset();
        }

        private void OnApplicationQuit()
        {
            InputListener.OnEscKeyGet -= _gameMenuPresenter.SwitchMenu;
        }
    }
    
    
}