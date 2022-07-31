using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public interface IGameLevelEventReceiver
    {
        void CompleteLevel();
        UniTask ConfirmLevelTask();
        UniTask CharacterGoTo(Vector3 destinationPosition);
        void ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition);
    }
}