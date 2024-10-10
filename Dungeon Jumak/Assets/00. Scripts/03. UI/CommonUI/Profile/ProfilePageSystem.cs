//Unity
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfilePageSystem : BaseProfileHandler
{
    enum Buttons { PreviousPage, NextPage }
    enum Texts { BonusRevenue, MaxOfflineDuration, InteriorScore }
    enum Sliders { ScoreSlider }

    [SerializeField] private GameObject[] m_ratingIcons;
    [SerializeField] private GameObject m_upgradeCompleteObj;

    private int m_page = 0;

    private void Start()
    {
        Init();
        g_ratingData.Data.CurrentRating = Mathf.Clamp(g_ratingData.Data.CurrentRating, 0, m_ratingIcons.Length - 1);
        UpdatePage(g_ratingData.Data.CurrentRating);
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));

        GetButton((int)Buttons.PreviousPage).gameObject.BindEvent(ToPreviousPage);
        GetButton((int)Buttons.NextPage).gameObject.BindEvent(ToNextPage);
    }

    private void ToPreviousPage(PointerEventData _data) => UpdatePage(m_page - 1);
    private void ToNextPage(PointerEventData _data) => UpdatePage(m_page + 1);

    private void UpdatePage(int _newPage)
    {
        m_page = Mathf.Clamp(_newPage, 0, m_ratingIcons.Length - 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (var icon in m_ratingIcons) icon.SetActive(false);
        var ratingData = RatingDataListHandler.Instance.GetRatingDataList().Data[m_page];

        SetBenefitsText(ratingData.bonusRevenue, ratingData.maxOfflineDuration);
        UpdateScoreText(g_ratingData.Data.CurrentInteriorScore, ratingData.goalInterirorScore, GetTMP((int)Texts.InteriorScore));
        UpdateSlider(GetSlider((int)Sliders.ScoreSlider), g_ratingData.Data.CurrentInteriorScore, ratingData.goalInterirorScore);

        m_ratingIcons[m_page].SetActive(true);
    }

    private void SetBenefitsText(float _bonusRevenue, float _maxOfflineDuration)
    {
        m_upgradeCompleteObj.SetActive(m_page < g_ratingData.Data.CurrentRating);
        if (!m_upgradeCompleteObj.activeSelf)
        {
            GetTMP((int)Texts.BonusRevenue).text = $"결제 수익 : +{_bonusRevenue * 100}%";

            int totalMinutes = Mathf.FloorToInt(_maxOfflineDuration);
            string displayTime = TimeCalculation(totalMinutes);

            GetTMP((int)Texts.MaxOfflineDuration).text = $"오프라인 최대 적용 시간 : {displayTime}";
        }
    }

    private string TimeCalculation(int totalMinutes)
    {
        if (totalMinutes >= 60)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return $"{hours}시간 {minutes}분";
        }
        else
        {
            return $"{totalMinutes}분";
        }
    }
}