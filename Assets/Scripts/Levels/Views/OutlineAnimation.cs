using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public abstract class OutlineAnimation : MonoBehaviour
    {
        public abstract UniTask HideOutline();
        public abstract UniTask ShowOutline(bool isComplete);
        public abstract UniTask FlickOutline(bool isComplete);
    }
}