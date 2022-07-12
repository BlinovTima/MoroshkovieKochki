using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class GameContext
    {
        private readonly List<int> _wrongLevelsIndexes;
        private readonly List<int> _passedLevelsIndexes;
        private int _score;

        private static GameContext _instance;

        private static GameContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameContext();
                
                return _instance;
            }
        }

        private GameContext()
        {
            _wrongLevelsIndexes = new List<int>();
            _passedLevelsIndexes = new List<int>();
            _score = 0;
        }

        public static void Reset()
        {
            _instance = new GameContext();
        }
        
        public static bool HasWrongLevels => Instance._wrongLevelsIndexes.Any();
        public static bool HasPassedLevels => Instance._passedLevelsIndexes.Any();
    }
}