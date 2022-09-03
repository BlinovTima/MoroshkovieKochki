using MoroshkovieKochki;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

public sealed class SkeletonAnimationEvents : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private string _eventName = "step";
    
    private bool _shouldPlayEffect;
    private AudioClip _audioClip;

    private void Start()
    {
        _skeletonAnimation.state.Event += OnEvent;
    }

   private void OnDestroy()
   {
       _skeletonAnimation.state.Event -= OnEvent;
   }

   private void OnEvent(TrackEntry trackentry, Event e)
   {
       if (_shouldPlayEffect && e.Data.Name == _eventName)
           AudioManager.PlayEffect(_audioClip);
   }
   
   public void SetupAudioClip(AudioClip audioClip)
   {
       _shouldPlayEffect = audioClip != null;
       _audioClip = audioClip;
   }
}
