using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MoroshkovieKochki
{
    public class GameMenuPresenter
    {
        private readonly GameMenu _gameMenu;

        public GameMenuPresenter(GameMenu gameMenu, Action onPlayButton)
        {
            _gameMenu = gameMenu;
            gameMenu.Init(onPlayButton, SwitchMenu, ApplyVolumeLevel);
            InputListener.OnEscKeyGet += SwitchMenu;
            LoadGameSettings();
        }

        public void ShowMenu(bool setPause = true)
        {
            Time.timeScale = setPause ? 0f : 1f;
            _gameMenu.Show();
            GameContext.AddGameState(GameState.Menu);
        }

        private void LoadGameSettings()
        {
            var value = PlayerSettings.GetMasterVolumeValue();
            _gameMenu.SetSlider(value);
            ApplyVolumeLevel(value);
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
            {
                HideMenu();
                AudioManager.ResumeSpeech();
            }
            else
            {
                AudioManager.PauseSpeech();
                ShowMenu();
            }
        }
        
        private void ApplyVolumeLevel(float value)
        {
            PlayerSettings.SafeMasterVolumeValue(value);
            AudioManager.SetMasterVolumeLerp(value);
        }
    }
}