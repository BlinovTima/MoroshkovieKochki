using NaughtyAttributes;
using UnityEngine;


namespace MoroshkovieKochki
{
    public abstract class InteractionItem : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField] private Transform _popupPivotPoint;
        [SerializeField] private Transform _characterInteractionPoint;

        [Header("Data")]
        [SerializeField] public Sprite Sprite;
        [Multiline]
        [SerializeField] public string Desription;
        [Multiline]
        [SerializeField] public string CustomQuestion;
        [SerializeField] public AudioClip SpeechAudio;
        public AudioClip IncorrectChoiceAudio;
        public AudioClip CorrectChoiceAudio;

        public Transform PopupPivotPoint => _popupPivotPoint;
        public Transform CharacterInteractionPoint => _characterInteractionPoint;
        public bool IsCompleted { get; protected set; }
        public bool IsRightAdvice { get; protected set; }

        public abstract void OnClick<TClickResult>(TClickResult value) where TClickResult : OnClickResult;
    }
}