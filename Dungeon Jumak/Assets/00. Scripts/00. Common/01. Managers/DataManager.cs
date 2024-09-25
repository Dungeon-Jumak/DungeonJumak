// System
using System;
using System.Collections.Generic;
using System.IO;

// Engine
using UnityEngine;

/// <summary>
/// 데이터를 관리하는 스크립트입니다. 런타임 중 데이터 값을 변경할 때에는 DataManager<PlayerData>.Instance.Data.CurPlayerLV++; 이렇게 사용하면 됩니다.
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
            }
            return instance;
        }
    }

    protected DataManager()
    {
        data = new T(); // 제네릭 데이터 초기화
    }

    #endregion

    // 파일 경로
    private static string dataFilePath = Application.persistentDataPath + "/GameData.json";

    // 데이터 인스턴스
    private T data;

    public T Data
    {
        get { return data; }
        set { data = value; }
    }

    [Serializable]
    public class SerializableData
    {
        public int id;  // 식별자
        public T data;   // 실제 데이터

        public SerializableData(int id, T data)
        {
            this.id = id;
            this.data = data;
        }
    }

    public class DataSaver
    {
        // 데이터 리스트를 JSON 형식으로 변환하여 파일에서 관리
        public static void SaveData(List<SerializableData> dataList)
        {
            string jsonData = JsonUtility.ToJson(dataList);
            File.WriteAllText(dataFilePath, jsonData);
        }
    }

    public class DataLoader
    {
        // 데이터 파일에서 데이터 리스트를 불러옴
        public static List<SerializableData> LoadData()
        {
            List<SerializableData> dataList = new List<SerializableData>();

            if (File.Exists(dataFilePath))
            {
                string jsonData = File.ReadAllText(dataFilePath);
                dataList = JsonUtility.FromJson<List<SerializableData>>(jsonData);
            }
            else
            {
                Debug.LogWarning("불러올 데이터가 없습니다.");
            }

            return dataList;
        }

        // 데이터 리스트를 Dictionary로 변환하여 반환
        public static Dictionary<int, T> GetDataDictionary(List<SerializableData> dataList)
        {
            Dictionary<int, T> dataDictionary = new Dictionary<int, T>();

            foreach (var data in dataList)
            {
                dataDictionary[data.id] = data.data;
            }

            return dataDictionary;
        }

        // Dictionary에서 ID를 사용하여 데이터를 찾아 반환
        public static T GetDataById(Dictionary<int, T> dataDictionary, int id)
        {
            T resultData;

            if (dataDictionary.TryGetValue(id, out resultData))
            {
                return resultData;
            }
            else
            {
                Debug.LogWarning("ID에 해당하는 데이터를 찾을 수 없습니다.");
                return default(T);
            }
        }
    }
}