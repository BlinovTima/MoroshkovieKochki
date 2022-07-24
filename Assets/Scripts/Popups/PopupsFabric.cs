using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoroshkovieKochki
{
    public class PopupsFabric : IDisposable
    {
        private readonly RectTransform _popupsParent;
        private GatherPopup _gatherPopupCache;
        private LevelTaskPopup _levelTaskPopupPopupCache;

        public PopupsFabric(RectTransform popupsParent)
        {
            _popupsParent = popupsParent;
        }

        public LevelTaskPopup GetPopup(GameLevel gameLevel)
        {
            if(!_levelTaskPopupPopupCache)
                _levelTaskPopupPopupCache = CreatePopup<LevelTaskPopup>();
                
            _levelTaskPopupPopupCache.Init(gameLevel.Description, gameLevel.ButtonLabel);
            return _levelTaskPopupPopupCache;
        }
        
        public ItemPopup GetPopup(InteractionItem interactionItem)
        {
            if (interactionItem is GatherItem data)
            {
                if(!_gatherPopupCache)
                    _gatherPopupCache = CreatePopup<GatherPopup>();
                
                _gatherPopupCache.Init(data);
                return _gatherPopupCache;
            }

            throw new NotImplementedException();
        }

        private T CreatePopup<T>() where T : PopupGeneric
        {
            var popupPrefab = Resources.Load<T>($"Popups/{typeof(T).Name}");
            var popup = Object.Instantiate(popupPrefab, _popupsParent);
            popup.gameObject.SetActive(false);
            
            return popup;
        }

        public void Dispose()
        {
            if(_gatherPopupCache)
                Object.Destroy(_gatherPopupCache);
        }
    }
}