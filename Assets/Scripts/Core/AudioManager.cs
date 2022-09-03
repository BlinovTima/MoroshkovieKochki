using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace MoroshkovieKochki
{
    public sealed class AudioManager : MonoBehaviour
    {
        private const string _masterVolumeName = "MasterVolume";
        private const string _musicVolumeName = "MusicVolume";
        private const string _speechVolumeName = "SpeechVolume";
        private const string _effectsVolumeName = "EffectsVolume";
        
        private const float _minValue = -80f;
        private float _defaultMasterVolume;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _effectsAudioSource;
        [SerializeField] private AudioSource _speechAudioSource;

        private static AudioManager _instance;
        private static bool _isAnimated;

        private static AudioManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    SetMasterVolumeLerp(PlayerSettings.GetMasterVolumeValue());
                }

                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static async UniTask SmoothMusicVolumeUp(float switchTime = 1f)
        {
            await Instance._audioMixer.DOSetFloat(_musicVolumeName, 0f, switchTime).AsyncWaitForCompletion();
        }
        
        public static async UniTask SetMusicVolumeDown(float switchTime)
        {
            await Instance._audioMixer.DOSetFloat(_musicVolumeName, _minValue, switchTime).AsyncWaitForCompletion();
        }

        public static void SetMasterVolumeLerp(float value)
        {
            var convertedValue = ConvertLerp(value);
            Instance._defaultMasterVolume = convertedValue;
            Instance._audioMixer.SetFloat(_masterVolumeName, convertedValue);
        }

        public static void PlayEffect(AudioClip audioClip) => 
            Instance._effectsAudioSource.PlayOneShot(audioClip);

        public static void PlaySpeech(AudioClip audioClip) => 
            Instance._speechAudioSource.PlayOneShot(audioClip);

        public static void ResumeSpeech() => 
            Instance._speechAudioSource.UnPause();

        public static void PauseSpeech() => 
            Instance._speechAudioSource.Pause();
        
        public static void StopSpeech() => 
            Instance._speechAudioSource.Stop();

        private static float ConvertLerp(float value)
        {
            var lerpedValue = Mathf.Lerp(0.5f, 1f, value);
            var result = Mathf.Lerp(_minValue, 0f, lerpedValue);
            return result;
        }
    }
}