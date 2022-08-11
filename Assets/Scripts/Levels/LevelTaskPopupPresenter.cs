﻿using Cysharp.Threading.Tasks;

namespace MoroshkovieKochki
{
    public sealed class LevelTaskPopupPresenter
    {
        private readonly PopupsFabric _popupsFabric;
        private OneButtonPopUp _taskPopupPopupCache;
       
        
        public LevelTaskPopupPresenter(PopupsFabric popupsFabric)
        {
            _popupsFabric = popupsFabric;
        }

        public async UniTask ConfirmLevelTask(GameLevel gameLevel)
        {
            _taskPopupPopupCache = _popupsFabric.GetPopup(gameLevel);
            _taskPopupPopupCache.Show().Forget();

            await UniTask.WaitUntil(() => _taskPopupPopupCache.IsTaskConfirmed);
        }

        public async UniTask HidePopup()
        {
            if(_taskPopupPopupCache != null)
                await _taskPopupPopupCache.Hide();
        }
    }
}