using System.IO;
using UnityEngine;

public class DataSaver
{
    public static void SaveData<T>(string filePath, T data)
    {
        // Json 변환
        string jsonData = JsonUtility.ToJson(data, true);

        // 파일 쓰기
        File.WriteAllText(filePath, jsonData);

        Debug.Log($"{typeof(T).Name} 데이터가 저장되었습니다.");
    }
}
