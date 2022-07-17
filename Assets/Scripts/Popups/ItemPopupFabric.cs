using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoroshkovieKochki
{
    public class ItemPopupFabric : IDisposable
    {
        private readonly RectTransform _popupsParent;
        private GatherPopup _gatherPopupCache;

        public ItemPopupFabric(RectTransform popupsParent)
        {
            _popupsParent = popupsParent;
        }
        
        public ItemPopup GetPopup(PopupData popupData)
        {
            if (popupData is GatherPopupData data)
            {
                if(!_gatherPopupCache)
                    _gatherPopupCache = CreatePopup<GatherPopup>();
                
                _gatherPopupCache.Init(data);
                return _gatherPopupCache;
            }

            throw new NotImplementedException();
        }

        private T CreatePopup<T>() where T : ItemPopup
        {
            var popupPrefab = Resources.Load<T>($"Popups/{typeof(T).Name}");
            var popup = UnityEngine.Object.Instantiate(popupPrefab, _popupsParent);
            return popup;
        }

        public void Dispose()
        {
            if(_gatherPopupCache)
                Object.Destroy(_gatherPopupCache);
        }
    }
}