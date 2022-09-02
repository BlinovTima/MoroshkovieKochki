using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace MoroshkovieKochki
{
    public sealed class AudioManager : MonoBehaviour
    {
        private const string _masterVolumeParametrName = "MasterVolume";
        private const float _minValue = -80f;
        private const float _defaultMasterVolume = 0f;

        [SerializeField] private AudioMixer _audioMixer;

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
        
        public static async UniTask SmoothMasterVolumeUp(float switchTime = 1f)
        {
            await Instance._audioMixer.DOSetFloat(_masterVolumeParametrName, _defaultMasterVolume, switchTime).AsyncWaitForCompletion();
        }
        
        public static async UniTask SmoothMasterVolumeDown(float switchTime)
        {
            await Instance._audioMixer.DOSetFloat(_masterVolumeParametrName, _minValue, switchTime).AsyncWaitForCompletion();
        }

        private static void SetMasterVolume(float value)
        {
            Instance._audioMixer.SetFloat(_masterVolumeParametrName, value);
            // _audioMixer.SetFloat("EffectsVolume", value);
            // _audioMixer.SetFloat("SpeechVolume", value);
            // _audioMixer.SetFloat("MusicVolume", value);
        }

        public static void SetMasterVolumeLerp(float value)
        {
            SetMasterVolume(Mathf.Lerp(_minValue, 0, value));
        }
    }
}