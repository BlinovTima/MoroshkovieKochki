using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace MoroshkovieKochki
{
    public sealed class AudioManager : MonoBehaviour
    {
        private const string _masterVolumeName = "MasterVolume";
        private const string _musicVolumeName = "MusicVolume";
        private const string _speechVolumeName = "SpeechVolume";
        private const string _sffectsVolumeName = "EffectsVolume";
        
        private const float _minValue = -80f;
        private const float _defaultMasterVolume = 0f;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _effectsAudioSource;

        private static AudioManager _instance;

        private static AudioManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<AudioManager>();

                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static async UniTask SmoothMasterVolumeUp(float switchTime = 1f)
        {
            Instance._audioMixer.DOSetFloat(_speechVolumeName, _defaultMasterVolume, switchTime);
            await Instance._audioMixer.DOSetFloat(_musicVolumeName, _defaultMasterVolume, switchTime).AsyncWaitForCompletion();
        }
        
        public static async UniTask SmoothMasterVolumeDown(float switchTime)
        {
            Instance._audioMixer.DOSetFloat(_speechVolumeName, _minValue, switchTime);
            await Instance._audioMixer.DOSetFloat(_musicVolumeName, _minValue, switchTime).AsyncWaitForCompletion();
        }

        private static void SetMusicAndSpeechVolume(float value)
        {
            Instance._audioMixer.SetFloat(_speechVolumeName, value);
            Instance._audioMixer.SetFloat(_musicVolumeName, value);
        }   
        

        public static void SetMusicAndSpeechVolumeLerp(float value)
        {
            SetMusicAndSpeechVolume(Mathf.Lerp(_minValue, 0, value));
        }

        public static void PlayEffect(AudioClip audioClip)
        {
            Instance._effectsAudioSource.PlayOneShot(audioClip);
        }
    }
}