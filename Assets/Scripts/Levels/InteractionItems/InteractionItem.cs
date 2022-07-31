using NaughtyAttributes;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class FootprintClickResult : OnClickResult
    {
        public Animals FootprintAnimal;
    }

    public sealed class GatherClickResult : OnClickResult
    {
        public bool ButtonClickValue;
    }
    public abstract class OnClickResult
    {
        
    }

    public abstract class InteractionItem : MonoBehaviour 
    {
    //       public T GetData<T>(string postfix = "") where T : ILevelData, new()
        [Header("Data")]
        [SerializeField] public Sprite Sprite;
        [ResizableTextArea]
        [SerializeField] public string Desription;


        [Header("Points")]
        [SerializeField] private Transform _popupPivotPoint;
        [SerializeField] private Transform _characterInteractionPoint;
        
        public Transform PopupPivotPoint => _popupPivotPoint;
        public Transform CharacterInteractionPoint => _characterInteractionPoint;
        public bool IsCompleted { get; protected set; }
        public bool IsRightAdvice { get; protected set; }

        public abstract void OnClick<TClickResult>(TClickResult value) where TClickResult : OnClickResult;
    }
}