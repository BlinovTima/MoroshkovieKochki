using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class FootprintsItem : InteractionItem
    {
        [Header("OnClick settings")]
        [SerializeField] private OutlineAnimation _outlineAnimation;
        [SerializeField] private Animals _thisFootprints;

        public Animals ThisFootprints => _thisFootprints;

        public override async void OnClick<TClickResult>(TClickResult value)
        {
            if(IsCompleted)
                return;

            if (value is FootprintClickResult clickResult)
                IsRightAdvice = _thisFootprints == clickResult.FootprintAnimal;

            IsCompleted = IsRightAdvice;

            await _outlineAnimation.ShowOutline(IsRightAdvice);
            
            if(IsRightAdvice)
                GameContext.AddScoreValue(1);
            else
                await _outlineAnimation.HideOutline();
        }
    }
}