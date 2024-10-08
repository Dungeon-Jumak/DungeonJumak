using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient Data", menuName = "Scriptable/Ingredient Data", order = int.MaxValue)]
public class IngredientData : ScriptableObject
{
    // 재료 넘버링
    // 대파 / 돼지고기 / 쌀 /  콩나물 / 다슬기 / 미꾸라지 / 수박 / 닭고기 / 버섯 / 소고기 / 오미자 / 굴 / 생선 / 미역 / 계피
    
    [Header("Ingredient ID")]
    public int id;

    [Header("Ingredient Name")]
    public string _name;

    [Header("Ingredient Count")]
    public int count;
}
