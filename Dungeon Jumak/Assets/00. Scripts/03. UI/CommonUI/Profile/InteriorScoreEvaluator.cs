using System.Collections.Generic;
using UnityEngine;

public class InteriorScoreEvaluator : MonoBehaviour
{
    private DataManager<RatingData> g_ratingData;
    private List<RatingDataSO> m_ratingDataList;

    private void Awake()
    {
        g_ratingData = DataManager<RatingData>.Instance;
        var ratingDataListHandler = RatingDataListHandler.Instance.GetRatingDataList();
        m_ratingDataList = ratingDataListHandler.Data;
    }

    private void Update()
    {
        EvaluateInteriorScore();
    }

    private void EvaluateInteriorScore()
    {
        if (g_ratingData.Data.CurrentRating < m_ratingDataList.Count &&
            m_ratingDataList[g_ratingData.Data.CurrentRating].goalInterirorScore <= g_ratingData.Data.CurrentInteriorScore)
        {
            g_ratingData.Data.CurrentRating++;
        }
    }
}