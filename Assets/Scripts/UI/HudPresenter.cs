using System;
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;

public class HudPresenter : IDisposable
{
    private readonly HudPanel _hudPanel;
    private bool _isAnimationInProcess;

    public HudPresenter(HudPanel hudPanel, Action onMenuButtonClick, Action startNewGame)
    {
        _hudPanel = hudPanel;
        _hudPanel.Init(0, onMenuButtonClick, startNewGame);
        GameContext.OnScoreUpdated += OnUpdateScore;
        GameContext.OnGameStateUpdated += UpdateVisuals;
    }

    private void UpdateVisuals(GameState state)
    {
        if (state.HasFlag(GameState.Menu)
            || state.HasFlag(GameState.HideHud))
        {
            _hudPanel.SetActiveScore(false);
            _hudPanel.SetActiveMenuButton(false);
            _hudPanel.SetActiveNewGameButton(false);
            return;
        }
        
        if (state.HasFlag(GameState.Play) && !_hudPanel.IsShown)
        {
            _hudPanel.SetActiveScore(true);
            _hudPanel.SetActiveMenuButton(true);
            _hudPanel.SetActiveNewGameButton(state.HasFlag(GameState.FinishLevel));
            Show().Forget();
        }
        else if (!state.HasFlag(GameState.Play) && _hudPanel.IsShown)
        {
            Hide().Forget();
        }
    }

    private void OnUpdateScore(int value) =>
        UpdateScore(value).Forget();
    
    private async UniTask UpdateScore(int value)
    {
        _isAnimationInProcess = true;
        await _hudPanel.AnimatedSetScore(value);
        _isAnimationInProcess = false;
    }

    private async UniTask Show()
    {
        await _hudPanel.Show();
    }

    private async UniTask Hide()
    {
        await UniTask.WaitUntil(() => _isAnimationInProcess);
        await _hudPanel.Hide();
    }

    public void Dispose()
    {
        GameContext.OnScoreUpdated -= OnUpdateScore;
        GameContext.OnGameStateUpdated -= UpdateVisuals;
    }
}