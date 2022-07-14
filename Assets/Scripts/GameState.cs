using System;

namespace DefaultNamespace
{
    [Flags]
    public enum GameState
    {
        None = 0,
        Menu = 1,
        Play = 2,
        CutScene = 4,
    }
}