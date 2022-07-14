using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MoroshkovieKochki
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playNewButtonText;
        [SerializeField] private TMP_Text _playAgainButtonText;
        [SerializeField] private TMP_Text _exitButtonText;
        [Space(10)]
        [SerializeField] private string _playNewButtonLabel;
        [SerializeField] private string _replayWrongButtonLabel;
        [SerializeField] private string _playAgainButtonLabel;
        [SerializeField] private string _exitButtonLabel;
        [Space(10)]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _exitButton;


        public void Init(Action onPlayButton, Action onPlayAgainButton)
        {
            _exitButton.onClick.AddListener(Application.Quit);
            _playButton.onClick.AddListener(onPlayButton.Invoke);
            _playAgainButton.onClick.AddListener(onPlayAgainButton.Invoke);
            
            SetupButtons();
        }

        private void OnEnable()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            _playAgainButton.gameObject.SetActive(GameContext.HasPassedLevels);
            
            _exitButtonText.text = _exitButtonLabel;
            _playNewButtonText.text = GameContext.HasWrongLevels ? _replayWrongButtonLabel : _playNewButtonLabel;
            _playAgainButtonText.text = _playAgainButtonLabel;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
        }
    }
}