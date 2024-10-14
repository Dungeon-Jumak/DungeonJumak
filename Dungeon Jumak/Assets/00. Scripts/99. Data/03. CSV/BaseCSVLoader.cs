#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public abstract class BaseCSVLoader : ICSVLoader
{
    public void LoadFromCSV(string _filePath)
    {
        LoadData(_filePath, false);
    }

    public void LoadFromCSVToList(string _filePath)
    {
        LoadData(_filePath, true);
    }

    private void LoadData(string _filePath, bool isList)
    {
        string[] m_lines = File.ReadAllLines(_filePath);
        string m_folderName = GetFolderName(_filePath);
        string m_folderPath = $"Assets/01. Resources/CSV_SO/{m_folderName}";
        ClearExistingAssets(m_folderPath);

        if (isList)
        {
            DataList dataList = CreateDataList();
            for (int i = 1; i < m_lines.Length; i++)
            {
                string[] m_fields = ParseCSVLine(m_lines[i]);
                AddDataToList(dataList, m_fields);
            }
            string m_assetPath = $"{m_folderPath}/{GetDataListName()}List.asset";
            AssetDatabase.CreateAsset(dataList, m_assetPath);
        }
        else
        {
            for (int i = 1; i < m_lines.Length; i++)
            {
                string[] m_fields = ParseCSVLine(m_lines[i]);
                ScriptableObject m_newObject = CreateScriptableObject();
                PopulateData(m_newObject, m_fields);
                string m_assetPath = GetAssetPath(m_newObject, m_folderPath);
                AssetDatabase.CreateAsset(m_newObject, m_assetPath);
            }
        }

        AssetDatabase.SaveAssets();
    }


    private string[] ParseCSVLine(string line)
    {
        return line.Split(',');
    }

    protected abstract string GetFolderName(string _filePath);
    protected abstract string GetAssetPath(ScriptableObject _newAsset, string _folderPath);
    protected abstract void PopulateData(ScriptableObject _asset, string[] _fields);
    protected abstract ScriptableObject CreateScriptableObject();

    // List
    protected abstract DataList CreateDataList();
    protected abstract void AddDataToList(DataList dataList, string[] fields);
    protected abstract string GetDataListName();

    private void ClearExistingAssets(string _folderPath)
    {
        if (AssetDatabase.IsValidFolder(_folderPath))
        {
            string[] m_existingAssets = AssetDatabase.FindAssets("t:ScriptableObject", new[] { _folderPath });

            foreach (var m_assetGuid in m_existingAssets)
            {
                string m_assetPath = AssetDatabase.GUIDToAssetPath(m_assetGuid);
                AssetDatabase.DeleteAsset(m_assetPath);
            }
        }
        else
        {
            AssetDatabase.CreateFolder("Assets/01. Resources/CSV_SO", Path.GetFileName(_folderPath));
        }
    }
}
#endif
