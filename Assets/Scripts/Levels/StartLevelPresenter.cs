
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;
using UnityEngine;

public sealed class StartLevelPresenter : GameLevelPresenter
{

    public override async UniTaskVoid ClickAction(RaycastHit2D raycastHit2D, Vector3 mousePosition)
    {
        await UniTask.Yield();
    }

    public override void PrepareLevelForStart()
    {
        _character.SetAnimationPreset(CharacterAnimationPreset.Default);
        base.PrepareLevelForStart();
        _character.SetScale(ScaleType.Title);
        _character.SetOrientationRight();
        _character.PlaySay();
        AudioManager.PlaySpeech(_gameLevel.TaskVoiceSpeech);
    }

    public override UniTask PlayIntro()
    {
        GameContext.AddGameState(GameState.HideHud);
        return base.PlayIntro();
    }

    public override UniTask PlayOutro()
    {
        GameContext.RemoveGameState(GameState.HideHud);
        return base.PlayOutro();
    }

    public void Dispose()
    {
      
    }
}