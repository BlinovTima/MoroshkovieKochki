using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoroshkovieKochki
{
    public sealed class PopupPresenter
    {
        private readonly ItemPopupFabric _itemPopupFabric;
        private ItemPopup _currentPopup;

        public void OnItemClick(RaycastHit2D hit2D)
        {
            ShowPopUp(hit2D).Forget();
        }

        private async UniTask ShowPopUp(RaycastHit2D hit2D)
        {
            var popupData = hit2D.collider.GetComponent<PopupData>();

            if (popupData)
            {
                if(_currentPopup)
                    await _currentPopup.Hide();
                
                var popup = _itemPopupFabric.GetPopup(popupData);
                    
                _currentPopup = popup;
                
                await popup.Show();
            }
        }

        public PopupPresenter(RectTransform popupsParent)
        {
            _itemPopupFabric = new ItemPopupFabric(popupsParent);
        }
        
    }
}