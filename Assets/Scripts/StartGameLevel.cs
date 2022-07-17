using System;
using UnityEngine;
using UnityEngine.UI;


namespace MoroshkovieKochki
{
    public sealed class StartGameLevel : GameLevel
    {
        [SerializeField] private Button _startGameButton;
        public override void Init(Action onLevelComplete, 
            Character character,
            PopupPresenter popupPresenter)
        {
            base.Init(onLevelComplete, character, popupPresenter);
            _startGameButton.onClick.AddListener(onLevelComplete.Invoke);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveAllListeners();
        }
    }
}