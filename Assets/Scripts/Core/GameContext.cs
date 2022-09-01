using System;

namespace MoroshkovieKochki
{
    public class GameContext
    {
        private int _score;
        private GameState _gameState;
        private static GameContext _instance;
        private int _passedLevels;

        private static GameContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameContext();
                
                return _instance;
            }
        }

        public static bool HasGameProgress => Instance._passedLevels > 0;
        public static bool GameIsFinished { get; set; }
        public static event Action<int> OnScoreUpdated = x => { };
        public static event Action<GameState> OnGameStateUpdated = x => { };
        public static void AddLevelPassed() => Instance._passedLevels += 1;
        
        
        public static void AddScoreValue(int value)
        {
            Instance._score += value;
            OnScoreUpdated.Invoke(Instance._score);
        }
        
        public GameContext()
        {
            GameIsFinished = false;
            _score = 0;
            _passedLevels = 0;
        }

        public static void Reset()
        {
            _instance = new GameContext();
        }
        
        
        #region Game state methods

        public static void AddGameState(GameState state)
        {
            Instance._gameState |= state;
            OnGameStateUpdated.Invoke(Instance._gameState);
        }  
        public static  bool HasGameState(GameState state)
        {
            return Instance._gameState.HasFlag(state);
        } 
        
        public static void RemoveGameState(GameState state)
        {
            Instance._gameState &= ~state;
            OnGameStateUpdated.Invoke(Instance._gameState);
        }
        
        public void ResetGameState()
        {
            _gameState = GameState.None;
        }

        #endregion
    }
}