using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class ScorePanel : MonoBehaviour
{
   [SerializeField] private CanvasGroup _canvasGroup;
   [SerializeField] private TMP_Text _scoreLabel;
   [SerializeField] private float _fadeTime = 1f;
   

   public void Init(int scoreValue)
   {
      SetScoreValue(scoreValue);
      SetActive(false);
   }

   public void SetScoreValue(int value)
   {
      _scoreLabel.text = value.ToString();
   }
   
   public void SetActive(bool isActive)
   {
      gameObject.SetActive(isActive);
   }
   
   public async UniTask Show()
   {
      if (gameObject.activeInHierarchy)
         return;

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
