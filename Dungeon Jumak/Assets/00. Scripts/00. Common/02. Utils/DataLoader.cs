using System.IO;
using UnityEngine;

public class DataLoader
{
    public static T LoadData<T>(string filePath) where T : new()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogWarning($"{typeof(T).Name} 데이터 파일이 없습니다. 새로 생성합니다.");
            return new T();
        }
    }
}
