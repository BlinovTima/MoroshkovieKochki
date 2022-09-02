using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class CursorEffectsPresenter
    {
        private readonly CursorEffectsView _cursorEffectsView;
        
        public CursorEffectsPresenter(CursorEffectsView cursorEffectsView)
        {
            _cursorEffectsView = cursorEffectsView;
            InputListener.OnLeftMouseButtonClick += ProduceLeftMouseClick;
        }

        private void ProduceLeftMouseClick(RaycastHit2D raycast, Vector3 mousePosition)
        {
            SetAudioEffect(raycast);
        }

        private void SetAudioEffect(RaycastHit2D raycast)
        {
            if(GameContext.HasGameState(GameState.OpenPopup))
                return;
            
            AudioClip clickAudio;
            
            if (raycast.collider.GetComponent<InteractionItem>())
                clickAudio = _cursorEffectsView.ItemMouseClick;
            else
                clickAudio = _cursorEffectsView.EmptyMouseClick;

            AudioManager.PlayEffect(clickAudio);
        }
    }
}