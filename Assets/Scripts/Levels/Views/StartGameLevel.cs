using UnityEngine;
using UnityEngine.UI;


namespace MoroshkovieKochki
{
    public sealed class StartGameLevel : GameLevel
    {
        [SerializeField] private Button _startGameButton;
        public override void Init(IGameLevelEventReceiver eventReceiver)
        {
            base.Init(eventReceiver);
            _startGameButton.onClick.AddListener(eventReceiver.LevelComplete);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveAllListeners();
        }
    }
}