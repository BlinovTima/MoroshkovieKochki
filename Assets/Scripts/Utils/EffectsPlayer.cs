using JetBrains.Annotations;
using MoroshkovieKochki;
using UnityEngine;

namespace Utils
{
    public sealed class EffectsPlayer : MonoBehaviour
    {
        [SerializeField] public AudioClip _effectAudio;

        [UsedImplicitly]
        public void PlayEffectOnce()
        {
            AudioManager.PlayEffect(_effectAudio);
        }
    }
}