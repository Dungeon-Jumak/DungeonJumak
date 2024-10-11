#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TestCSVLoader : BaseCSVLoader
{
    protected override string GetFolderName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    protected override string GetAssetPath(ScriptableObject newAsset, string folderPath)
    {
        TestMonster testAsset = newAsset as TestMonster;
        return $"{folderPath}/{testAsset.data.MonsterID}.asset";
    }

    protected override void PopulateData(ScriptableObject asset, string[] fields)
    {
        TestMonster testAsset = asset as TestMonster;
        testAsset.data = new MonsterData
        {
            MonsterName = fields[0],
            MonsterID = int.Parse(fields[1]),
            MonsterHealth = float.Parse(fields[2])
        };
    }

    protected override ScriptableObject CreateScriptableObject()
    {
        return ScriptableObject.CreateInstance<TestMonster>();
    }

    protected override DataList CreateDataList()
    {
        return null;
    }

    protected override void AddDataToList(DataList dataList, string[] fields)
    {
    }

    protected override string GetDataListName()
    {
        return null;
    }
}
#endif
