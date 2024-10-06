using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    [Header("CSV 파일 경로")]
    public string m_filePath;
    public enum ObjectType 
    { 
        Test, 
        Monster
    }

    public ObjectType m_objectType;

    /// <summary>
    /// CSV 데이터를 로드합니다.
    /// </summary>
    public void LoadDataFromCSV()
    {
        ICSVLoader loader = null;

        switch (m_objectType)
        {
            case ObjectType.Test:
                loader = new TestCSVLoader();
                break;
            case ObjectType.Monster:
                loader = new TestMonsterCSVLoader();
                break;
        }

        if (loader != null)
        {
            loader.LoadFromCSV(m_filePath);
            Debug.Log("CSV Loading Complete!");
        }
    }
}
