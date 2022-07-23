using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public interface IGameLevelEventReceiver
    {
        void LevelComplete();
        UniTask CharacterGoTo(Vector3 destinationPosition);
        UniTask GatherClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition);
    }
}