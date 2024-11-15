using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encyclopedia", menuName = "Scriptable/Encyclopedia/Data", order = int.MaxValue)]
public class EncyclopediaSO : ScriptableObject
{
    public Category category;
    public Sprite Icon;
    public string EntryName;
    public string Desc;

    public enum Category
    {
        Monster,
        Customer
    }
}
