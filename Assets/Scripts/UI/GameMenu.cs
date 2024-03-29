﻿using System;
using TMPro;
using UnityEngine;
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
        [Space(7)]
        [SerializeField] private Button _switchSoundButton;
        [SerializeField] private Slider _masterVolumeSlider;
        private Action<float> _onVolumeLevelChanged;
        private float _cachedValue;

        public bool IsMenuOpen => gameObject.activeInHierarchy;
        
        public void Init(Action playButton, Action onResumeButton, Action<float> onVolumeLevelChanged)
        {
            _onVolumeLevelChanged = onVolumeLevelChanged;
            _exitButton.onClick.AddListener(Application.Quit);
            _playButton.onClick.AddListener(playButton.Invoke);
            _resumeButton.onClick.AddListener(onResumeButton.Invoke);
            _switchSoundButton.onClick.AddListener(SwitchSound);
            _masterVolumeSlider.onValueChanged.AddListener(OnSetVolumeSlider);
            SetupButtons();
        }

        private void SwitchSound()
        {
            if (_masterVolumeSlider.value > 0)
            {
                _cachedValue = _masterVolumeSlider.value;
                OnSetVolumeSlider(0);
                SetSlider(0);
            }
            else
            {
                OnSetVolumeSlider(_cachedValue);
                SetSlider(_cachedValue);
                _cachedValue = 0;
            }
        }

        public void SetSlider(float value) =>
            _masterVolumeSlider.value = value;

        private void OnSetVolumeSlider(float value)
        {
            _onVolumeLevelChanged.Invoke(value);
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