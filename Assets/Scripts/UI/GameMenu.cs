using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace MoroshkovieKochki
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playNewButtonText;
        [SerializeField] private TMP_Text _exitButtonText;
        [SerializeField] private Transform _resumeButtonContainer;
        
        [Space(10)]
        [SerializeField] private string _playNewButtonLabel;
        [SerializeField] private string _replayWrongButtonLabel;
        [SerializeField] private string _exitButtonLabel;
        [Space(10)]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Slider _masterVolumeSlider;

        public bool IsMenuOpen => gameObject.activeInHierarchy;
        
        public void Init(Action playButton, Action onResumeButton)
        {
            _exitButton.onClick.AddListener(Application.Quit);
            _playButton.onClick.AddListener(playButton.Invoke);
            _resumeButton.onClick.AddListener(onResumeButton.Invoke);
            _masterVolumeSlider.onValueChanged.AddListener(ApplyVolumeLevel);
            SetupButtons();
        }

        private void ApplyVolumeLevel(float value)
        {
            AudioManager.SetMasterVolumeLerp(value);
        }


        private void OnEnable()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            _exitButtonText.text = _exitButtonLabel;
            _playNewButtonText.text = GameContext.GameIsFinished ? _replayWrongButtonLabel : _playNewButtonLabel;
        }

        public void Show()
        {
            _resumeButtonContainer.gameObject.SetActive(GameContext.HasGameProgress);
            SetActive(true);
        }
        
        public void Hide()
        {
            SetActive(false);
        }

        private void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.RemoveAllListeners();
        }
    }
}