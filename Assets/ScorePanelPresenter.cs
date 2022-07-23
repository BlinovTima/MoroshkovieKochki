using System;
using Cysharp.Threading.Tasks;
using MoroshkovieKochki;

public class ScorePanelPresenter : IDisposable
{
    private readonly ScorePanel _scorePanel;

    public ScorePanelPresenter(ScorePanel scorePanel)
    {
        _scorePanel = scorePanel;
        _scorePanel.Init(0);
        GameContext.OnScoreUpdated += UpdateScore;
        GameContext.OnGameStateUpdated += UpdateVisuals;
    }

    private void UpdateVisuals(GameState state)
    {
        if (state.HasFlag(GameState.Menu))
        {
            _scorePanel.SetActive(false);
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
        _scorePanel.SetScoreValue(value);
    }

    private async UniTask Show()
    {
        await _scorePanel.Show();
    }

    private async UniTask Hide()
    {
        await _scorePanel.Hide();
    }

    public void Dispose()
    {
        GameContext.OnScoreUpdated -= UpdateScore;
        GameContext.OnGameStateUpdated -= UpdateVisuals;
    }
}