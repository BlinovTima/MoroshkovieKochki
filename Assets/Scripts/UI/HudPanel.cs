using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class HudPanel : MonoBehaviour
{
   [SerializeField] private CanvasGroup _canvasGroup;
   [SerializeField] private TMP_Text _scoreLabel;
   [SerializeField] private float _fadeTime = 1f;
   [SerializeField] private Button _menuButton;
   [SerializeField] private Button _playAgainButton;
   

   public void Init(int scoreValue, Action menuButtonClick, Action startNewGame)
   {
      _menuButton.onClick.AddListener(menuButtonClick.Invoke);
      _playAgainButton.onClick.AddListener(startNewGame.Invoke);
      
      SetScoreValue(scoreValue);
      gameObject.SetActive(false);
   }

   public void SetScoreValue(int value)
   {
      _scoreLabel.text = value.ToString();
   }
   
   public void SetActiveScore(bool isActive) => 
      gameObject.SetActive(isActive);
   public void SetActiveMenuButton(bool isActive) => 
      _menuButton.gameObject.SetActive(isActive);
   public void SetActiveNewGameButton(bool isActive) => 
      _playAgainButton.gameObject.SetActive(isActive);
   
   public async UniTask Show()
   {
      _canvasGroup.alpha = 0f;
      gameObject.SetActive(true);
         
      var sequence = DOTween.Sequence();
      sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _fadeTime));
      
      await sequence.AsyncWaitForCompletion();
   }
   
   public async UniTask Hide()
   {
      if (!gameObject.activeInHierarchy)
         return;
      
      var sequence = DOTween.Sequence();
      sequence.Append(DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _fadeTime));
      sequence.AppendCallback(() => gameObject.SetActive(false));
      
      await sequence.AsyncWaitForCompletion();
   }
}
