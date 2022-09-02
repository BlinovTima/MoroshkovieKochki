using System;
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;

public class HudPresenter : IDisposable
{
    private readonly HudPanel _hudPanel;

    public HudPresenter(HudPanel hudPanel, Action onMenuButtonClick, Action startNewGame)
    {
        _hudPanel = hudPanel;
        _hudPanel.Init(0, onMenuButtonClick, startNewGame);
        GameContext.OnScoreUpdated += UpdateScore;
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

    private void UpdateScore(int value)
    {
        _hudPanel.SetScoreValue(value);
    }

    private async UniTask Show()
    {
        await _hudPanel.Show();
    }

    private async UniTask Hide()
    {
        await _hudPanel.Hide();
    }

    public void Dispose()
    {
        GameContext.OnScoreUpdated -= UpdateScore;
        GameContext.OnGameStateUpdated -= UpdateVisuals;
    }
}