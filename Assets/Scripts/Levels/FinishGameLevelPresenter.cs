using Cysharp.Threading.Tasks;
using MoroshkovieKochki;
using UnityEngine;

public sealed class FinishGameLevelPresenter : GameLevelPresenter
{

    public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
    {
        await UniTask.Yield();
    }

    public override void PrepareLevelForStart()
    {
        _character.SetAnimationPreset(CharacterAnimationPreset.Default);
        base.PrepareLevelForStart();
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