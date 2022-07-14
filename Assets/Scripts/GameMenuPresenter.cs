using System;

namespace MoroshkovieKochki
{
    public class GameMenuPresenter
    {
        private GameMenu _gameMenu;

        public GameMenuPresenter(GameMenu gameMenu, Action onPlayButton, Action onPlayAgainButton)
        {
            _gameMenu = gameMenu;
            gameMenu.Init(onPlayButton, onPlayAgainButton);
        }

        public void ShowMenu()
        {
            _gameMenu.SetActive(true);
        }  
        
        public void HideMenu()
        {
            _gameMenu.SetActive(false);
        }
        
        public void SwitchMenu()
        {
            _gameMenu.SetActive(!_gameMenu.gameObject.activeInHierarchy);
        }
    }
}