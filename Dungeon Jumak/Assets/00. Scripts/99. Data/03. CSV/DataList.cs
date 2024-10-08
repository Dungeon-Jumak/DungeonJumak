using System.Collections.Generic;
using UnityEngine;

public class DataList : ScriptableObject { }

//public class RatingDataList : DataList
//{
//    public List<RatingDataSO> Data = new List<RatingDataSO>();
//}

public class TestMonsterDataList : DataList
{
    public List<MonsterData> Data = new List<MonsterData>();
}