using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoroshkovieKochki
{
    public class PopupsFabric : IDisposable
    {
        private readonly RectTransform _popupsParent;
        private TwoButtonsPopUp _twoButtonsPopUpCache;
        private InfoPopup _infoPopupCache;
        private OneButtonPopUp _oneButtonPopUpPopupCache;

        public PopupsFabric(RectTransform popupsParent)
        {
            _popupsParent = popupsParent;
        }

        public OneButtonPopUp GetPopup(GameLevel gameLevel)
        {
            if(!_oneButtonPopUpPopupCache)
                _oneButtonPopUpPopupCache = CreatePopup<OneButtonPopUp>();
                
            _oneButtonPopUpPopupCache.Init(gameLevel.Description, gameLevel.ButtonLabel);
            return _oneButtonPopUpPopupCache;
        }
        
        public ItemPopup GetPopup(InteractionItem interactionItem)
        {
            if (interactionItem is MosquitoItem)
            {
                if(!_infoPopupCache)
                    _infoPopupCache = CreatePopup<InfoPopup>();
                
                _infoPopupCache.Init(interactionItem);
                return _infoPopupCache;
            }
            
            if (interactionItem is BushItem 
                || interactionItem is GatherItem)
            {
                if(!_twoButtonsPopUpCache)
                    _twoButtonsPopUpCache = CreatePopup<TwoButtonsPopUp>();
                
                _twoButtonsPopUpCache.Init(interactionItem, true);
                return _twoButtonsPopUpCache;
            }
            
            if (interactionItem is FootprintsItem footprintsItemData)
            {
                if(!_infoPopupCache)
                    _infoPopupCache = CreatePopup<InfoPopup>();
                
                _infoPopupCache.Init(footprintsItemData);
                return _infoPopupCache;
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
            if(_twoButtonsPopUpCache)
                Object.Destroy(_twoButtonsPopUpCache);
        }
    }
}