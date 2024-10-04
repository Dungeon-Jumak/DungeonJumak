using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StBtnSet : MonoBehaviour
{
    [SerializeField] private GameObject[] m_bgmButtons;
    [SerializeField] private GameObject[] m_sfxButtons;
    [SerializeField] private GameObject[] m_pushButtons;
    [SerializeField] private GameObject[] m_nightPushButtons;

    private DataManager<AudioData> g_audioData;
    private DataManager<SettingData> g_settingData;

    private void Awake()
    {
        g_audioData = DataManager<AudioData>.Instance;
        g_settingData = DataManager<SettingData>.Instance;
    }

    private async void OnEnable()
    {
        await UniTask.Delay(1);
        SetBgmButton();
        SetSfxButton();
        SetPushButton();
        SetNightPushButton();
    }

    /// <summary>
    /// BGM 버튼 상태를 설정합니다.
    /// </summary>
    private void SetBgmButton()
    {
        bool isPlayBGM = g_audioData.Data.IsPlayBGM;
        m_bgmButtons[isPlayBGM ? 1 : 0].SetActive(false);
    }

    /// <summary>
    /// SFX 버튼 상태를 설정합니다.
    /// </summary>
    private void SetSfxButton()
    {
        bool isPlaySFX = g_audioData.Data.IsPlaySFX;
        m_sfxButtons[isPlaySFX ? 1 : 0].SetActive(false);
    }

    /// <summary>
    /// Push Message 버튼 상태를 설정합니다.
    /// </summary>
    private void SetPushButton()
    {
        bool isPushEnable = g_settingData.Data.IsPushEnable;
        m_pushButtons[isPushEnable ? 1 : 0].SetActive(false);
    }

    /// <summary>
    /// 야간 Push Message 버튼 상태를 설정합니다.
    /// </summary>
    private void SetNightPushButton()
    {
        bool isNightPushEnable = g_settingData.Data.IsNightPushEnable;
        m_nightPushButtons[isNightPushEnable ? 1 : 0].SetActive(false);
    }
}
