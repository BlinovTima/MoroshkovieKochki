using System;
using UnityEngine;

namespace MoroshkovieKochki
{
    public class ItemPopupFabric
    {
        private readonly RectTransform _popupsParent;
        private GatherPopup popupCache;

        public ItemPopupFabric(RectTransform popupsParent)
        {
            _popupsParent = popupsParent;
        }
        
        public ItemPopup GetPopup(PopupData popupData)
        {
            if (popupData is GatherPopupData data)
            {
                if(!popupCache)
                    popupCache = CreatePopup<GatherPopup>();
                
                popupCache.Init(data);
                return popupCache;
            }

            throw new NotImplementedException();
        }

        private T CreatePopup<T>() where T : ItemPopup
        {
            var popupPrefab = Resources.Load<T>($"Popups/{typeof(T).Name}");
            var popup = UnityEngine.Object.Instantiate(popupPrefab, _popupsParent);
            return popup;
        }
        
    }
}