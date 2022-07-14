using System.Collections.Generic;
using System.Linq;

namespace MoroshkovieKochki
{
    public class GameContext
    {
        private readonly List<int> _wrongLevelsIndexes;
        private readonly List<int> _passedLevelsIndexes;
        private int score;

        private static GameContext _instance;
        private GameState _gameState;
        
        private static GameContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameContext();
                
                return _instance;
            }
        }

        public static bool HasWrongLevels => Instance._wrongLevelsIndexes.Any();

        public static bool HasPassedLevels => Instance._passedLevelsIndexes.Any();


        private GameContext()
        {
            _wrongLevelsIndexes = new List<int>();
            _passedLevelsIndexes = new List<int>();
            score = 0;
        }

        public static void Reset()
        {
            _instance = new GameContext();
        }


        #region Game state methods

        public static void AddGameState(GameState state)
        {
            _instance._gameState |= state;
        }  
        public static  bool HasGameState(GameState state)
        {
            return _instance._gameState.HasFlag(state);
        } 
        
        public static void RemoveGameState(GameState state)
        {
            _instance._gameState &= ~state;
        }
        
        public void ResetGameState()
        {
            _gameState = GameState.None;
        }

        #endregion
    }
}