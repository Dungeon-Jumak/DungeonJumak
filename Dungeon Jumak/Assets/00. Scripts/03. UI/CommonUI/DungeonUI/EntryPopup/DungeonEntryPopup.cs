using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonEntryPopup : UI_PopUp
{
    enum Buttons { Confirm }

    private int stageIndex;
    private DataManager<DungeonPopupData> g_DungeonPopupData;

    private void Awake()
    {
        Init();
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
        BaseDungeonListPopup.OnSweepBtnClicked += SetStageIndex;
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Confirm).gameObject.BindEvent(EntryDungeon);
    }

    private void SetStageIndex(int _StageIndex)
    {
        stageIndex = _StageIndex;
    }

    private void EntryDungeon(PointerEventData _data)
    {
        g_DungeonPopupData.Data.CurrentStage = stageIndex;
        string sceneName = $"Stage{stageIndex}";
        SceneManager.LoadSceneAsync(sceneName);
    }
}
