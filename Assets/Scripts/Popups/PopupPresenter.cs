using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class PopupPresenter : IDisposable
    {
        private readonly PopupsFabric _popupsFabric;
        private ItemPopup _currentPopup;
        private string _currentPopupDataName;

        public ItemPopup CurrentPopup => _currentPopup;

        public PopupPresenter(PopupsFabric popupsFabric)
        {
            _popupsFabric = popupsFabric;
        }

        public bool IsPointInPopup(Vector2 mousePoint)
        {
            if (!IsPopupOpen)
                return false;

            return _currentPopup.BoundsRect.IsPointInRect(mousePoint);
        }

        public bool IsPopupOpen =>
            _currentPopup && _currentPopup.gameObject.activeInHierarchy;
        
        public bool NeedCloseCurrentPopup(InteractionItem interactionItem)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && interactionItem.gameObject.name != _currentPopupDataName;
        }

        private bool IsCachedPopup(InteractionItem interactionItem)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && interactionItem.gameObject.name == _currentPopupDataName;
        }

        public async UniTask CloseCurrentPopup()
        {
            if (!_currentPopup)
                return;

            _currentPopupDataName = null;
            await _currentPopup.Hide();
        }

        public async UniTask ShowPopUp(InteractionItem interactionItem)
        {
            if(IsCachedPopup(interactionItem) && _currentPopup.ActiveInHierarchy)
                return;
            
            _currentPopupDataName = interactionItem.gameObject.name;
            
            if (_currentPopup && _currentPopup.ActiveInHierarchy)
                await _currentPopup.Hide();

            _currentPopup = _popupsFabric.GetPopup(interactionItem);

            var screenPoint = Camera.main.WorldToScreenPoint(interactionItem.PopupPivotPoint.position);
            await _currentPopup.Show(screenPoint);
        }

        public void Dispose()
        {
            _popupsFabric.Dispose();
            _currentPopupDataName = null;
        }
    }
}