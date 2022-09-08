using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class CursorEffectsView : MonoBehaviour
    {
        [SerializeField] public AudioClip ItemMouseClick;
        [SerializeField] public AudioClip EmptyMouseClick;
        [SerializeField] public GameObject CkickEffect;
        [SerializeField] private float _zEffectDepth = -6f;

        public void CreateEffect(Vector2 raycastPoint)
        {
            transform.position = new Vector3(raycastPoint.x, raycastPoint.y, _zEffectDepth);
            Instantiate(CkickEffect, transform);
        }
    }
}