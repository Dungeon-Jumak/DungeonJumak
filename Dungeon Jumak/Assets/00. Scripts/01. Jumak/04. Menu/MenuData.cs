using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu Data", menuName = "Scriptable/Menu Data", order = int.MaxValue)]
public class MenuData : ScriptableObject
{
    [Header("Menu ID")]
    public int id;

    [Header("Menu Name")]
    public string _name;

    [Header("Menu Category (0 : 메인, 1 : 사이드, 2 : 음료)")]
    [Range(0,2)] public int category;

    [Header("Menu Level (proficiency : 1 ~ 5)"), Range(1, 5)]
    public int level;

    [Header("Menu Prices")]
    public int[] prices;

    [Header("Menu Cooking Times")]
    public float[] cookingTimes;

    [Header("Menu Sprite")]
    public Sprite sprite;

    [Header("Empty Bowl Sprite")]
    public Sprite emptySprite;

    [Header("Ingredient Data")]
    public IngredientData[] ingredients;
}
