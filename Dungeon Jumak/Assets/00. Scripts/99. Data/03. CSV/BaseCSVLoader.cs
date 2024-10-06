using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public abstract class BaseCSVLoader<T> : ICSVLoader where T : ScriptableObject
{
    private readonly List<T> m_scriptableObjects = new();
    public IReadOnlyList<T> ScriptableObjects => m_scriptableObjects.AsReadOnly();

    public void LoadFromCSV(string _filePath)
    {
        string[] m_lines = File.ReadAllLines(_filePath);
        string m_folderName = GetFolderName(_filePath);
        string m_folderPath = $"Assets/01. Resources/CSV_SO/{m_folderName}";
        ClearExistingAssets(m_folderPath);

        for (int i = 1; i < m_lines.Length; i++)
        {
            string[] m_fields = m_lines[i].Split(',');

            T m_newObject = ScriptableObject.CreateInstance<T>();
            PopulateData(m_newObject, m_fields);

            string m_assetPath = GetAssetPath(m_newObject, m_folderPath);

            AssetDatabase.CreateAsset(m_newObject, m_assetPath);
            AssetDatabase.SaveAssets();

            m_scriptableObjects.Add(m_newObject);
        }
    }

    protected abstract string GetFolderName(string _filePath);
    protected abstract string GetAssetPath(T _newAsset, string _folderPath);
    protected abstract void PopulateData(T _asset, string[] _fields);

    private void ClearExistingAssets(string _folderPath)
    {
        if (AssetDatabase.IsValidFolder(_folderPath))
        {
            string[] m_existingAssets = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { _folderPath });

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
