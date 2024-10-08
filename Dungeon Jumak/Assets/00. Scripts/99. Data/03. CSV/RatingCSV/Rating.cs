using UnityEngine;

[System.Serializable]
public class RatingDataSO
{
    public string ratingName;
    public float bonusRevenue;
    public float maxOfflineDuration;
    public int goalInterirorScore; 
}

[CreateAssetMenu(fileName = "RatingData", menuName = "Scriptable/CSV/RatingData", order = int.MaxValue)]
public class Rating : ScriptableObject
{
    public RatingDataSO data;
}