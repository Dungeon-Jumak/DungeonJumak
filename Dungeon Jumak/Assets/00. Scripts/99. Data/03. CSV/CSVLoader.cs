#if UNITY_EDITOR
using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    [Header("CSV 파일 경로")]
    public string m_filePath;
    public enum ObjectType
    {
        Rating,
        Monster
    }

    public enum SaveType
    {
        Individual,
        List
    }

    public ObjectType m_objectType;
    public SaveType m_saveType;

    /// <summary>
    /// CSV 데이터를 로드합니다.
    /// </summary>
    public void LoadDataFromCSV()
    {
        ICSVLoader loader = CreateLoader();

        if (loader != null)
        {
            if (m_saveType == SaveType.Individual)
            {
                loader.LoadFromCSV(m_filePath);
            }
            else
            {
                loader.LoadFromCSVToList(m_filePath);
            }
            Debug.Log("CSV Loading Complete!");
        }
    }

    private ICSVLoader CreateLoader()
    {
        switch (m_objectType)
        {
            case ObjectType.Rating:
                return new RatingCSVLoader();
            case ObjectType.Monster:
                return new TestCSVLoader();
            default: return null;
        }
    }
}
#endif