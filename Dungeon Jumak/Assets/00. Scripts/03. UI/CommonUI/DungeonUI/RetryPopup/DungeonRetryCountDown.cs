using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UIElements;

public class DungeonRetryCountDown : UI_PopUp
{
    enum Texts { CountDown }

    [SerializeField] private float startTime = 10f;
    private bool isEnable;

    private void Awake()
    {
        Init();
    }

    private void OnDisable()
    {
        isEnable = false;        
    }

    private async void Start()
    {
        isEnable = true;
        await StartCountdown(startTime);
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
    }


    private async UniTask StartCountdown(float time)
    {
        float currentTime = time;

        while (currentTime > 0 && isEnable)
        {
            GetTMP((int)Texts.CountDown).text = Mathf.Ceil(currentTime).ToString();

            await UniTask.Delay(1000);

            currentTime -= 1f;
        }

        if (currentTime == 0)
        {
            EndCountDown();
        }
    }

    private void EndCountDown()
    {
        GetTMP((int)Texts.CountDown).text = "0";
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonGameOverPopUp");
    }
}
