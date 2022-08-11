
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;
using UnityEngine;

public sealed class StartLevelPresenter : GameLevelPresenter
{

    public override void ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
    {
      
    }

    public override UniTask PlayIntro()
    {
        GameContext.AddGameState(GameState.HideScore);
        return base.PlayIntro();
    }

    public override UniTask PlayOutro()
    {
        GameContext.RemoveGameState(GameState.HideScore);
        return base.PlayOutro();
    }

    public void Dispose()
    {
      
    }
}