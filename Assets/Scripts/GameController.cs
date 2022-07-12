using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameMenu _gameMenu;
        [SerializeField] private Character _character;
        [SerializeField] private InputListener _inputListener;
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
              _gameMenuPresenter = new GameMenuPresenter(_gameMenu, 
                  () => StartNextLevel().Forget(), 
                  () => ResetAnPlayAgain().Forget());

              _inputListener.OnEscKeyGet += _gameMenuPresenter.SwitchMenu;
              
              _gameMenuPresenter.ShowMenu();
        }

        private async UniTask StartNextLevel()
        {
            var nextLevel = GetNextLevel();
            await _windowSwitcher.SwitchWindow(() =>
            {
                _gameMenuPresenter.HideMenu();

                if (_currentLevel != null)
                    Destroy(_currentLevel.gameObject);

                _currentLevel = Instantiate(nextLevel, _levelParent);
                _currentLevel.Init(() => StartNextLevel().Forget());

                _character.transform.SetParent(_currentLevel.CharacterParent);
            });
        }
        
        private async UniTask ResetAnPlayAgain()
        {
         
        }
    }
    
    
}