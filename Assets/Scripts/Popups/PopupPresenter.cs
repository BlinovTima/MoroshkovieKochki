using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MoroshkovieKochki
{
    public sealed class PopupPresenter : IDisposable
    {
        private readonly ItemPopupFabric _itemPopupFabric;
        private ItemPopup _currentPopup;
        private string _currentPopupDataName;


        public bool IsPointInPopup(Vector2 mousePoint)
        {
            if (!_currentPopup || !_currentPopup.gameObject.activeInHierarchy)
                return false;

            return _currentPopup.BoundsRect.IsPointInRect(mousePoint);
        }
        
        public bool NeedCloseCurrentPopup(InteractionItem interactionItem)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && interactionItem.gameObject.name != _currentPopupDataName;
        } 
        
        private bool IsCachedPopup(InteractionItem interactionItem)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && interactionItem.gameObject.name == _currentPopupDataName;
        }
        
        public void CloseCurrentPopup()
        {
            if (!_currentPopup)
                return;

            _currentPopupDataName = null;
            _currentPopup.Hide().Forget();
        }

        public async UniTask ShowPopUp(InteractionItem interactionItem)
        {
            if(IsCachedPopup(interactionItem))
                return;
            
            _currentPopupDataName = interactionItem.gameObject.name;
            
            if (_currentPopup && _currentPopup.ActiveInHierarchy)
                await _currentPopup.Hide();

            _currentPopup = _itemPopupFabric.GetPopup(interactionItem);

            var screenPoint = Camera.main.WorldToScreenPoint(interactionItem.PopupPivotPoint.position);
            await _currentPopup.Show(screenPoint);
        }

        public PopupPresenter(RectTransform popupsParent)
        {
            _itemPopupFabric = new ItemPopupFabric(popupsParent);
        }

        public void Dispose()
        {
            _itemPopupFabric.Dispose();
            _currentPopupDataName = null;
        }
    }
}