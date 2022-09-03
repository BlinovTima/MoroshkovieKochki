using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace MoroshkovieKochki
{
    public sealed class StartGameLevel : GameLevel
    {
        [SerializeField] private Button _startGameButton;
        public override void Init(IGameLevelEventReceiver eventReceiver,
            float characterXBound)
        {
            base.Init(eventReceiver, characterXBound);
            
            _startGameButton.onClick.AddListener(eventReceiver.CompleteLevel);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveAllListeners();
        }
    }
}