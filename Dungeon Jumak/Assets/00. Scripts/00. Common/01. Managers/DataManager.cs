// Engine
using UnityEngine;

/// <summary>
/// Data
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataManager<T> where T : new()
{
    #region Singleton

    private static DataManager<T> instance;
    public static DataManager<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager<T>();
                instance.LoadData(); // 데이터 로드
            }
            return instance;
        }
    }

    #endregion

    private T data;

    public T Data
    {
        get { return data; }
        set
        {
            data = value;
            SaveData();
        }
    }

    // 파일명 설정
    private string filePath = Application.persistentDataPath + $"/{typeof(T).Name}.json";

    // DataSaver.cs 데이터 저장 함수 연결
    public void SaveData()
    {
        DataSaver.SaveData(filePath, data);
    }

    // DataLoader.cs 데이터 로드 함수 연결
    public void LoadData()
    {
        data = DataLoader.LoadData<T>(filePath);
    }
}
