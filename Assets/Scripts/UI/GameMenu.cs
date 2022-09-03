using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utils;


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
        private Action<float> _onVolumeLevelChanged;

        public bool IsMenuOpen => gameObject.activeInHierarchy;
        
        public void Init(Action playButton, Action onResumeButton, Action<float> onVolumeLevelChanged)
        {
            _onVolumeLevelChanged = onVolumeLevelChanged;
            _exitButton.onClick.AddListener(Application.Quit);
            _playButton.onClick.AddListener(playButton.Invoke);
            _resumeButton.onClick.AddListener(onResumeButton.Invoke);
            _masterVolumeSlider.onValueChanged.AddListener(SetVolumeSlider);
            SetupButtons();
        }

        private void SetVolumeSlider(float value)
        {
            _onVolumeLevelChanged.Invoke(value);
            _masterVolumeSlider.value = value;
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