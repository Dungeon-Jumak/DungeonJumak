using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteriorScoreHandler : BaseProfileHandler
{
    enum Texts { InteriorScore }
    enum Sliders { ScoreSlider }

    private void Start()
    {
        Init();
        SetData();
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
    }


    private void SetData()
    {
        var ratingDataList = RatingDataListHandler.Instance.GetRatingDataList();
        var currentRatingIndex = g_ratingData.Data.CurrentRating;
        var currentInteriorScore = g_ratingData.Data.CurrentInteriorScore;

        UpdateSliderAndText(ratingDataList, currentRatingIndex, currentInteriorScore);
    }

    private void UpdateSliderAndText(RatingDataList _ratingDataList, int _currentRatingIndex, float _currentInteriorScore)
    {
        var goalScore = _ratingDataList.Data[_currentRatingIndex].goalInterirorScore;
        var slider = GetSlider((int)Sliders.ScoreSlider);

        if (_currentRatingIndex == 0)
        {
            slider.minValue = 0;
        }
        else
        {
            slider.minValue = _ratingDataList.Data[_currentRatingIndex - 1].goalInterirorScore;
        }

        slider.minValue = _currentRatingIndex == 0 ? 0 : _ratingDataList.Data[_currentRatingIndex - 1].goalInterirorScore;

        UpdateSlider(slider, _currentInteriorScore, goalScore);
        UpdateScoreText(_currentInteriorScore, goalScore, GetTMP((int)Texts.InteriorScore));
    }

    //Debug
    //public void onClick() => g_ratingData.Data.CurrentInteriorScore += 20;
}