using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumakData
{
    public int currentLevel = 0;
    public int maxLevel = 7;

    public int maxWaitingCount = 5;
    public int maxSeatCount = 2;

    //주문 속도
    public float orderSpeed = 3f;

    //결제 속도
    public float paymentCooltime = 4f;

    //청소 속도
    public float cleaningTime = 6f;
}

public class DefaultJumakData
{
    //식사 속도
    public float eatingTime = 10f;
}

public class YardData
{
    public string ID { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int level { get; set; }
    public int unlockScore { get; set; }
    public string unlockCondition { get; set; }
    public int cost { get; set; }
    public string function { get; set; }
    public float functionValue { get; set; }
    public int restaurantScore { get; set; }
    public bool unlock { get; set; }
}

public class JumakDataManager : MonoBehaviour
{
    private CSVReader csvReader;

    private const string YARD_DATA_FILE_NAME = "YardData";
    private const string DEFAULT_JUMAK_DATA_FILE_NAME = "DefaultJumakData";

    private List<YardData> yardDatas;
    private DefaultJumakData jumakData;

    private void Awake()
    {
        csvReader = new CSVReader();
    }

    public float GetEatingTime(string _ID)
    {
        yardDatas = csvReader.ReadCSVFile<List<YardData>>(YARD_DATA_FILE_NAME);
        jumakData = csvReader.ReadCSVFile<DefaultJumakData>(DEFAULT_JUMAK_DATA_FILE_NAME);

        for (int i = 0; i < yardDatas.Count; i++)
        {
            if (_ID == yardDatas[i].ID)
            {
                return jumakData.eatingTime * yardDatas[i].functionValue + jumakData.eatingTime;
            }

        }

        return 0f;
    }

}
