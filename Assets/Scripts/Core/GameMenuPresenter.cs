using System;
using UnityEngine;

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
            var isMenuActive = _gameMenu.gameObject.activeInHierarchy;
            
            if(isMenuActive)
            {
                Time.timeScale = 1f;
                GameContext.RemoveGameState(GameState.Menu);
            }
            else
            {
                Time.timeScale = 0f;
                GameContext.AddGameState(GameState.Menu);
            }
            
            _gameMenu.SetActive(!isMenuActive);
            
            
        }
    }
}