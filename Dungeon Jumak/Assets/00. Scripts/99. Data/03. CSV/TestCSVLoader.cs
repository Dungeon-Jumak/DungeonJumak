using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TestCSVLoader : BaseCSVLoader<test>
{
    protected override string GetFolderName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    protected override string GetAssetPath(test newAsset, string folderPath)
    {
        return $"{folderPath}/{newAsset.characterID}.asset";
    }

    protected override void PopulateData(test asset, string[] fields)
    {
        asset.characterName = fields[0];
        asset.characterID = int.Parse(fields[1]);
        asset.characterHealth = float.Parse(fields[2]);
    }
}
