using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaSlotData : UI_PopUp
{
    enum Texts { E_Name }
    enum Images { E_Icon }

    private EncyclopediaSO entry;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void SetEntry(EncyclopediaSO _newEntry)
    {
        entry = _newEntry;
    }

    public EncyclopediaSO GetEntry()
    {
        return entry;
    }

    public void UpdateSlot(string _name, Sprite _icon)
    {
        GetTMP((int)Texts.E_Name).text = $"<color=black>{_name}</color>";
        GetImage((int)Images.E_Icon).sprite = _icon;
        GetImage((int)Images.E_Icon).color = Color.white;
    }
}
