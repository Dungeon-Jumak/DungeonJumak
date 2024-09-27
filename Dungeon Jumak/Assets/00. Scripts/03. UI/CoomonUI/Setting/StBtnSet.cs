using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StBtnSet : MonoBehaviour
{
    [SerializeField] private GameObject[] BgmBtn;
    [SerializeField] private GameObject[] SfxBtn;


    private GlobalData data;
    private void Awake()
    {
        data = DataManager.Instance.data;
    }

    private async void OnEnable()
    {
        await UniTask.Delay(1);

        SetBgmBtn();
        SetSfxBtn();
    }

    private void SetBgmBtn()
    {
        //if (data.g_onBgm)
        //{
        //    BgmBtn[1].SetActive(false);
        //}
        //else
        //{
        //    BgmBtn[0].SetActive(false);
        //}
    }

    private void SetSfxBtn()
    {
        //if (data.g_onSfx)
        //{
        //    SfxBtn[1].SetActive(false);
        //}
        //else
        //{
        //    SfxBtn[0].SetActive(false);
        //}
    }
}
