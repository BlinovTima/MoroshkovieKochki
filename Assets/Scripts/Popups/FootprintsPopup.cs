using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoroshkovieKochki
{
    internal class FootprintsPopup : ItemPopup, IDisposable
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _desription;
        [SerializeField] private Animals _rightAnswer;

        public Animals RightAnswer => _rightAnswer;

        public void Init(FootprintsItem item)
        {
            Dispose();
            _image.sprite = item.Sprite;
            _desription.text = item.Desription;
            _rightAnswer = item.ThisFootprints;
        }

        public void Dispose()
        {
        }
        
        private void OnDestroy()
        {
            Dispose();
        }
    }
}