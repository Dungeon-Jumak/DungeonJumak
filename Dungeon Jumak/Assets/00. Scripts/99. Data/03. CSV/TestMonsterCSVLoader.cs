using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TestMonsterCSVLoader : BaseCSVLoader<TestMonster>
{
    protected override string GetFolderName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    protected override string GetAssetPath(TestMonster newAsset, string folderPath)
    {
        return $"{folderPath}/{newAsset.MonsterName}.asset";
    }

    protected override void PopulateData(TestMonster asset, string[] fields)
    {
        asset.MonsterName = fields[0];
        asset.MonsterID = int.Parse(fields[1]);
        asset.MonsterHealth = float.Parse(fields[2]);
    }
}
