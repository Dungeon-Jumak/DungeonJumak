// System
using System;
using System.Collections.Generic;
using System.IO;

// Engine
using UnityEngine;

public class DataManager<T>
{
    // 파일 경로
    private static string dataFilePath = Application.persistentDataPath + "/GameData.json";

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