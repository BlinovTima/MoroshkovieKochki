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


        public PopupPresenter(PopupsFabric popupsFabric)
        {
            _popupsFabric = popupsFabric;
        }

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