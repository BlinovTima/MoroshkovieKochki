using UnityEngine;


namespace MoroshkovieKochki
{
    public abstract class PopupData : MonoBehaviour
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public string Desription;
        [SerializeField] public string CustomQuestion;
        [SerializeField] private Transform _popupPivotPoint;
        [SerializeField] private Transform _characterInteractionPoint;
        
        public Transform PopupPivotPoint => _popupPivotPoint;
        public Transform CharacterInteractionPoint => _characterInteractionPoint;

        public abstract void OnClick(bool value);
    }
}