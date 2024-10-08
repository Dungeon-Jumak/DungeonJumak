using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    [Header("전체 재료 배열")]
    [SerializeField] private IngredientData[] ingredientData;

    private Dictionary<IngredientData, int> ingredientDictionary;

    private void Start()
    {
        ingredientDictionary = new Dictionary<IngredientData, int>();

        for (int i = 0; i < ingredientData.Length; i++)
        {
            UpdateIngredientInDictionary(ingredientData[i], 1);
        }
    }

    public void UpdateIngredientInDictionary(IngredientData _ingredientData, int _count)
    {
        ingredientDictionary.Add(_ingredientData, _count);
    }

    public Dictionary<IngredientData, int> GetIngredientDictionary()
    {
        return ingredientDictionary;
    }


    /// <summary>
    /// 재료를 특정 갯수만큼 더하기 위한 메소드
    /// </summary>
    /// <param name="ingredientDatam"></param>
    /// <param name="_increase"></param>
    public void AddIngredientCount(IngredientData _ingredientData, int _increase)
    {
        ingredientDictionary[_ingredientData] += _increase;
    }

    /// <summary>
    /// 재료를 특정 갯수만큼 빼기 위한 메소드
    /// </summary>
    /// <param name="ingredientDatam"></param>
    /// <param name="_decrease"></param>
    public void SubIngredientCount(IngredientData _ingredientData, int _decrease)
    {
        ingredientDictionary[_ingredientData] -= _decrease;
    }
}
