using System;
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;

public class HudPresenter : IDisposable
{
    private readonly HudPanel _hudPanel;

    public HudPresenter(HudPanel hudPanel, Action onMenuButtonClick)
    {
        _hudPanel = hudPanel;
        _hudPanel.Init(0, onMenuButtonClick);
        GameContext.OnScoreUpdated += UpdateScore;
        GameContext.OnGameStateUpdated += UpdateVisuals;
    }

    private void UpdateVisuals(GameState state)
    {
        if (state.HasFlag(GameState.Menu)
            || state.HasFlag(GameState.HideScore))
        {
            _hudPanel.SetActive(false);
            return;
        }
            
        if (state.HasFlag(GameState.Play))
        {
            Show().Forget();
        }
        else
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