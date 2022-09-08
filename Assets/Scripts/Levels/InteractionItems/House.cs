using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class House : MonoBehaviour
    {
        public MosquitoHouse MosquitoHouse;
        [SerializeField] private OutlineAnimation _outlineAnimation;

        public void FlickOutline(bool isRightAdvice) =>
            _outlineAnimation.FlickOutline(isRightAdvice);
    }
}