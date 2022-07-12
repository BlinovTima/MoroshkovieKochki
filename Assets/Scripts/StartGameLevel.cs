using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public sealed class StartGameLevel : GameLevel
    {
        [SerializeField] private Button _startGameButton;
        public override void Init(Action onLevelComplete)
        {
            _startGameButton.onClick.AddListener(onLevelComplete.Invoke);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveAllListeners();
        }
    }
}