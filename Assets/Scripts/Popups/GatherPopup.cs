using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoroshkovieKochki
{
    public sealed class GatherPopup : ItemPopup, IDisposable
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _desription;
        [SerializeField] private TMP_Text _customQuestion;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        
        private bool _shouldGather;
        
        public  void Init(GatherItem item)
        {
            Dispose();
            
            _image.sprite = item.Sprite;
            _desription.text = item.Desription;
            _customQuestion.text = item.CustomQuestion;
            _shouldGather = item.ShouldSayYes;
            
            _yesButton.onClick.AddListener(() => item.OnClick(true));
            _yesButton.onClick.AddListener(() => Hide().Forget());
            
            _noButton.onClick.AddListener(() => item.OnClick(false));
            _noButton.onClick.AddListener(() => Hide().Forget());
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
    }
}