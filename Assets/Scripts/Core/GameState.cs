using System;

namespace MoroshkovieKochki
{
    [Flags]
    public enum GameState
    {
        None = 0,
        Menu = 1,
        Play = 2,
        CutScene = 4,
        HideHud = 8,
        FinishLevel = 16,
        OpenPopup = 32,
    }
}