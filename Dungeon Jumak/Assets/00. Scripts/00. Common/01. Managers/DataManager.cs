// System
using System;
using System.Collections.Generic;
using System.IO;

// Engine
using UnityEngine;

public class DataManager<T>
{
    // ���� ���
    private static string dataFilePath = Application.persistentDataPath + "/GameData.json";

    [Serializable]
    public class SerializableData
    {
        public int id;  // �ĺ���
        public T data;   // ���� ������

        public SerializableData(int id, T data)
        {
            this.id = id;
            this.data = data;
        }
    }

    public class DataSaver
    {
        // ������ ����Ʈ�� JSON �������� ��ȯ�Ͽ� ���Ͽ��� ����
        public static void SaveData(List<SerializableData> dataList)
        {
            string jsonData = JsonUtility.ToJson(dataList);
            File.WriteAllText(dataFilePath, jsonData);
        }
    }

    public class DataLoader
    {
        // ������ ���Ͽ��� ������ ����Ʈ�� �ҷ���
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
                Debug.LogWarning("�ҷ��� �����Ͱ� �����ϴ�.");
            }

            return dataList;
        }

        // ������ ����Ʈ�� Dictionary�� ��ȯ�Ͽ� ��ȯ
        public static Dictionary<int, T> GetDataDictionary(List<SerializableData> dataList)
        {
            Dictionary<int, T> dataDictionary = new Dictionary<int, T>();

            foreach (var data in dataList)
            {
                dataDictionary[data.id] = data.data;
            }

            return dataDictionary;
        }

        // Dictionary���� ID�� ����Ͽ� �����͸� ã�� ��ȯ
        public static T GetDataById(Dictionary<int, T> dataDictionary, int id)
        {
            T resultData;

            if (dataDictionary.TryGetValue(id, out resultData))
            {
                return resultData;
            }
            else
            {
                Debug.LogWarning("ID�� �ش��ϴ� �����͸� ã�� �� �����ϴ�.");
                return default(T);
            }
        }
    }
}