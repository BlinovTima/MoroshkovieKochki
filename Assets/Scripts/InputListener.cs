using System;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class InputListener : MonoBehaviour
    {
        public event Action OnEscKeyGet = () => { };
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameContext.HasGameState(GameState.CutScene))
            {
                OnEscKeyGet.Invoke();
            }
        }
    }
}