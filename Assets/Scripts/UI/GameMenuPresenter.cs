using System;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class GameMenuPresenter
    {
        private readonly GameMenu _gameMenu;

        public GameMenuPresenter(GameMenu gameMenu, Action onPlayButton)
        {
            _gameMenu = gameMenu;
            gameMenu.Init(onPlayButton, SwitchMenu);
        }

        public void ShowMenu()
        {
            Time.timeScale = 0f;
            _gameMenu.Show();
            GameContext.AddGameState(GameState.Menu);
        }  
        
        public void HideMenu()
        {
            _gameMenu.Hide();
            Time.timeScale = 1f;
            GameContext.RemoveGameState(GameState.Menu);
        }
        
        public void SwitchMenu()
        {
            if(!GameContext.HasGameProgress)
                return;
            
            if(_gameMenu.IsMenuOpen)
                HideMenu();
            else
                ShowMenu();
        }
    }
}