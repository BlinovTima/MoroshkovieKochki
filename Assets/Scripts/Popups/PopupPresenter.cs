using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


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
        
        public bool NeedCloseCurrentPopup(PopupData popupData)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && popupData.gameObject.name != _currentPopupDataName;
        } 
        
        private bool IsCachedPopup(PopupData popupData)
        {
           return !string.IsNullOrEmpty(_currentPopupDataName) && popupData.gameObject.name == _currentPopupDataName;
        }
        
        public void CloseCurrentPopup()
        {
            if (!_currentPopup)
                return;

            _currentPopupDataName = null;
            _currentPopup.Hide().Forget();
        }

        public async UniTask ShowPopUp(PopupData popupData)
        {
            if(IsCachedPopup(popupData))
                return;
            
            _currentPopupDataName = popupData.gameObject.name;
            
            if (_currentPopup && _currentPopup.ActiveInHierarchy)
                await _currentPopup.Hide();

            _currentPopup = _itemPopupFabric.GetPopup(popupData);

            var screenPoint = Camera.main.WorldToScreenPoint(popupData.PopupPivotPoint.position);
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