using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoroshkovieKochki
{
    public sealed class LevelsFabric
    {
        private readonly Transform _levelParent;
        private readonly PopupPresenter _popupPresenter;
        private readonly LevelTaskPopupPresenter _levelTaskPopupPresenter;
        private readonly Character _character;
        private readonly Action _onCompleteLevelAction;
        private Queue<Func<GameLevelPresenter>> _levelsQueue;


        public LevelsFabric(Transform levelParent, 
            RectTransform popupsParent,
            Character character,
            Action onCompleteLevelAction)
        {
            _onCompleteLevelAction = onCompleteLevelAction;
            _character = character;
            _levelParent = levelParent;
            
            var popupsFabric = new PopupsFabric(popupsParent);
            _popupPresenter = new PopupPresenter(popupsFabric);
            _levelTaskPopupPresenter = new LevelTaskPopupPresenter(popupsFabric);
        }

        public void InitLevels()
        {
            _levelsQueue = new Queue<Func<GameLevelPresenter>>();
            _levelsQueue.Enqueue(InitLevel<BirdsGameLevel, BirdsLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<MosquitoGameLevel, MosquitoLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<MushroomsGameLevel, MushroomsLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<FinishGameLevel, FinishGameLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<FootprintsGameLevel, FootprintsLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<BerriesGameLevel, BerriesLevelPresenter>);
            _levelsQueue.Enqueue(InitLevel<StartGameLevel, StartLevelPresenter>);
        }

        private TPresenter InitLevel<TView, TPresenter>() where TView : GameLevel, new() where TPresenter : GameLevelPresenter, new()
        {
            var view = CreateLevel<TView>();
            var presenter = new TPresenter();
            presenter.Init(view, _onCompleteLevelAction, _character, _popupPresenter, _levelTaskPopupPresenter);
            
            return presenter;
        }

        private T CreateLevel<T>() where T : GameLevel
        {
            var prefab = Resources.Load<T>($"Levels/{typeof(T).Name}");

            if (!prefab)
                throw new Exception($"Prefab {typeof(T).Name} doesn't found. Rename prefab.");
            
            var instantiate = Object.Instantiate(prefab, _levelParent);
            return instantiate;
        }

        public bool GetNextLevel(out GameLevelPresenter gameLevelPresenter)
        {
            gameLevelPresenter = null;

            if (_levelsQueue.Count > 0)
            {
                var hasNextLevel = _levelsQueue.Dequeue();
                var level = hasNextLevel.Invoke();
                gameLevelPresenter = level;
                return true;
            }
            
            return false;
        }
        
    }
}