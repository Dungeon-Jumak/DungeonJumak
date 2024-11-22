using UnityEngine;

public enum ItemType
{
    GreenOnion,
    Pork,
    Rice,
    SoybeanSprouts,
    FreshwaterSnail,
    Loach,
    Watermelon,
    Chicken,
    Mushroom,
    Beef,
    SchisandraBerry,
    Oyster,
    Fish,
    Seaweed,
    Cinnamon,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable/InvItemData", order = int.MaxValue)]
public class InventoryData : ScriptableObject
{
    public ItemType ItemType;
    [Tooltip("아이템 ID")] public int ID;
    [Tooltip("인벤토리에 표시될 아이콘")] public Sprite ItemIcon;
    [Tooltip("아이템 이름")] public string Name;
    [Tooltip("아이템 설명")] public string Desc;
}