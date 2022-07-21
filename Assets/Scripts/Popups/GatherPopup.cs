using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoroshkovieKochki
{
    public sealed class GatherPopup : ItemPopup
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _desription;
        [SerializeField] private TMP_Text _customQuestion;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        
        private bool _shouldGather;
        
        public  void Init(GatherPopupData popupData)
        {
            _image.sprite = popupData.Sprite;
            _desription.text = popupData.Desription;
            _customQuestion.text = popupData.CustomQuestion;
            _shouldGather = popupData.ShouldGather;
            
            _yesButton.onClick.AddListener(() => popupData.OnClick(true));
            _yesButton.onClick.AddListener(() => Hide().Forget());
            
            _noButton.onClick.AddListener(() => popupData.OnClick(false));
            _noButton.onClick.AddListener(() => Hide().Forget());
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
    }
}