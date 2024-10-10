using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseProfileHandler : UI_PopUp
{
    protected DataManager<RatingData> g_ratingData;

    protected virtual void Awake()
    {
        g_ratingData = DataManager<RatingData>.Instance;
    }

    protected void UpdateScoreText(float _currentScore, float _goalScore, TextMeshProUGUI _scoreText)
    {
        string color = _currentScore < _goalScore ? "red" : "green";
        _scoreText.text = $"<color={color}>{_currentScore}</color> / {_goalScore}";
    }

    protected void UpdateSlider(Slider _slider, float _currentScore, float _goalScore)
    {
        _slider.maxValue = _goalScore;
        _slider.value = _currentScore;
    }
}