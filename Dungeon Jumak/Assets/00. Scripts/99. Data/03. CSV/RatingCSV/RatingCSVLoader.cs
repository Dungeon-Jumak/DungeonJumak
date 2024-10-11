#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class RatingCSVLoader : BaseCSVLoader
{
    protected override string GetFolderName(string _filePath)
    {
        return Path.GetFileNameWithoutExtension(_filePath);
    }

    protected override string GetAssetPath(ScriptableObject _newAsset, string _folderPath)
    {
        Rating ratingAsset = _newAsset as Rating;
        return $"{_folderPath}/{ratingAsset.data.ratingName}.asset";
    }

    protected override void PopulateData(ScriptableObject _asset, string[] _fields)
    {
        Rating ratingAsset = _asset as Rating;
        ratingAsset.data = new RatingDataSO
        {
            ratingName = _fields[0],
            bonusRevenue =float.Parse(_fields[1]),
            maxOfflineDuration = float.Parse(_fields[2]),
            goalInterirorScore = int.Parse(_fields[3]),
        };
    }

    protected override ScriptableObject CreateScriptableObject()
    {
        return ScriptableObject.CreateInstance<Rating>();
    }

    protected override DataList CreateDataList()
    {
        return ScriptableObject.CreateInstance<RatingDataList>();
    }

    protected override void AddDataToList(DataList dataList, string[] _fields)
    {
        RatingDataList ratingDataList = dataList as RatingDataList;
        ratingDataList.Data.Add(new RatingDataSO
        {
            ratingName = _fields[0],
            bonusRevenue = float.Parse(_fields[1]),
            maxOfflineDuration = float.Parse(_fields[2]),
            goalInterirorScore = int.Parse(_fields[3]),
        });
    }

    protected override string GetDataListName()
    {
        return nameof(RatingDataSO);
    }
}
#endif