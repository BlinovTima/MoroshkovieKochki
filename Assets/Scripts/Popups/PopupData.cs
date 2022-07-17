using UnityEngine;


namespace MoroshkovieKochki
{
    public abstract class PopupData : MonoBehaviour
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public string Desription;
        [SerializeField] public string CustomQuestion;
    }
}