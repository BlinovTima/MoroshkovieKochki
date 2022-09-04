using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MoroshkovieKochki;
using Spine.Unity;
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
   [SerializeField] private AudioClip _scoreAddingSound;
   [Header("Score animation settings")] 
   [SerializeField] private float _scoreAnimationTime = 1f;
   [SerializeField] private float _maxScale = 1.7f;

   public bool IsShown => gameObject.activeInHierarchy;

   public void Init(int scoreValue, Action menuButtonClick, Action startNewGame)
   {
      _menuButton.onClick.AddListener(menuButtonClick.Invoke);
      _playAgainButton.onClick.AddListener(startNewGame.Invoke);
      
      SetScore(scoreValue);
      gameObject.SetActive(false);
   }

   public async UniTask AnimatedSetScore(int value)
   {
      var isComplete = false;
      var scoreTimeHalf = _scoreAnimationTime / 2;
      AudioManager.PlayEffect(_scoreAddingSound);

      _scoreLabel.DOFade(0f, scoreTimeHalf);
      _scoreLabel.transform.DOScale(_maxScale, scoreTimeHalf).OnComplete(
         () =>
         {
            SetScore(value);
            _scoreLabel.color = Color.green;
            _scoreLabel.transform.localScale = Vector3.one * 0.5f;
            _scoreLabel.DOColor(Color.white, scoreTimeHalf);
            _scoreLabel.DOFade(1f, scoreTimeHalf);
            _scoreLabel.transform.DOScale(1f, scoreTimeHalf).SetEase(Ease.OutElastic)
               .OnComplete(() => isComplete = true);
         });

      await UniTask.WaitUntil(() => isComplete);
   }

   public void SetActiveScore(bool isActive) => 
      gameObject.SetActive(isActive);

   public void SetActiveMenuButton(bool isActive) => 
      _menuButton.gameObject.SetActive(isActive);

   public void SetActiveNewGameButton(bool isActive) => 
      _playAgainButton.gameObject.SetActive(isActive);

   private void SetScore(int value) => 
      _scoreLabel.text = value.ToString();

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
