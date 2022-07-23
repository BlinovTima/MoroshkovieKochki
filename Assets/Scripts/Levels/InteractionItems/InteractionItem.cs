using UnityEngine;


namespace MoroshkovieKochki
{
    
    
    public abstract class InteractionItem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] public Sprite Sprite;
        [SerializeField] public string Desription;
        [SerializeField] public string CustomQuestion;
        [SerializeField] public bool ShouldSayYes;
        
        [Header("Points")]
        [SerializeField] private Transform _popupPivotPoint;
        [SerializeField] private Transform _characterInteractionPoint;
        
        public Transform PopupPivotPoint => _popupPivotPoint;
        public Transform CharacterInteractionPoint => _characterInteractionPoint;
        public bool IsCompleted { get; protected set; }
        public bool IsRightAdvice { get; protected set; }
        
        public abstract void OnClick(bool value);
    }
}