using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RatingDataList", menuName = "Scriptable/CSV/RatingDataList", order = int.MaxValue)]
public class RatingDataList : DataList
{
    public List<RatingDataSO> Data = new List<RatingDataSO>();
}

