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

        public void OnItemClick(RaycastHit2D hit2D)
        {
            var popupData = hit2D.collider.GetComponent<PopupData>();
            
            if(popupData)
            {
                ShowPopUp(popupData).Forget();
            }
            else if(_currentPopup)
            {
                _currentPopupDataName = null;
                _currentPopup.Hide().Forget();
            }
        }

        private async UniTask ShowPopUp(PopupData popupData)
        {
            if (!string.IsNullOrEmpty(_currentPopupDataName) 
                && popupData.gameObject.name == _currentPopupDataName)
                return;

            _currentPopupDataName = popupData.gameObject.name;
            
            if (_currentPopup && _currentPopup.ActiveInHierarchy)
                await _currentPopup.Hide();

            _currentPopup = _itemPopupFabric.GetPopup(popupData);

            await _currentPopup.Show();
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