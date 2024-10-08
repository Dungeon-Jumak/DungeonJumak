using UnityEditor;
using UnityEngine;

public class CSVLoaderEditor : EditorWindow
{
    private string m_filePath;
    private CSVLoader.ObjectType m_objectType;
    private CSVLoader.SaveType m_saveType;

    [MenuItem("Tools/CSV Loader")]
    public static void ShowWindow()
    {
        GetWindow<CSVLoaderEditor>("CSV Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Load CSV and Create Scriptable Objects", EditorStyles.boldLabel);

        m_filePath = EditorGUILayout.TextField("CSV File Path", m_filePath);
        m_objectType = (CSVLoader.ObjectType)EditorGUILayout.EnumPopup("Object Type", m_objectType);
        m_saveType = (CSVLoader.SaveType)EditorGUILayout.EnumPopup("Save Type", m_saveType);

        if (GUILayout.Button("Load CSV"))
        {
            LoadCSV();
        }
    }

    /// <summary>
    /// CSV 데이터를 로드합니다.
    /// </summary>
    private void LoadCSV()
    {
        if (string.IsNullOrEmpty(m_filePath))
        {
            Debug.LogError("파일 경로가 비어 있습니다.");
            return;
        }

        var loader = new CSVLoader
        {
            m_filePath = m_filePath,
            m_objectType = m_objectType,
            m_saveType = m_saveType
        };

        loader.LoadDataFromCSV();
    }
}